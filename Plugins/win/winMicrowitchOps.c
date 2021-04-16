#include "microwitchOps.h"

#define UNICODE 1  // use WCHAR API's

#include <windows.h>
#include <objbase.h>
#include <shlobj.h>
#include <shellapi.h>
#include <winuser.h>

#define true 1
#define false 0

/* entry points */

int
GetDriveInfo(const char *drive, char *dst, int max)
{
  char volume_name[256];
  char volume_system[256];
  DWORD serial, length, flags;

  if (!GetVolumeInformationA(drive, volume_name, sizeof volume_name, &serial, &length, &flags, volume_system, sizeof volume_system)) {
    return 0;
  }
  strncpy(dst, volume_name, max);
  return 1;
}

int
MicrobitDevice(char *dst, int max)
{
  DWORD drives;
  DWORD mask;
  char drive_letter[4];
  char buf[256];
  int ret;

  drive_letter[0] = 'X';
  drive_letter[1] = ':';
  drive_letter[2] = '\\';
  drive_letter[3] = '\0';
  drives = GetLogicalDrives();
  for (int c = 'A', mask = 1; c <= 'Z'; c ++, mask <<= 1) {
    if (!(drives & mask)) {
      continue;
    }
    drive_letter[0] = c;
    ret = GetDriveInfo(drive_letter, buf, sizeof buf);
    if ( ret ) {
      if (!strcmp(buf, "MICROBIT")) {
	strncpy(dst, drive_letter, max);
	return 1;
      }
    }
  }
  return 0;
}

#define INITGUID
#include <setupapi.h>
#include <guiddef.h>

DEFINE_GUID(GUID_DEVINTERFACE_COMPORT, 0x86e0d1e0, 0x8089, 0x11d0, 0x9c, 0xe4, 0x08, 0x00, 0x3e, 0x30, 0x1f, 0x73);

int
EnumerateComPorts(char *dst, int max)
{
  int i;
  char *ptr = dst;
  int org_max = max;
  TCHAR tmpbuf[512];

  SP_DEVINFO_DATA DeviceInfoData = {sizeof(SP_DEVINFO_DATA)};
  HDEVINFO hDevInfo = SetupDiGetClassDevs(&GUID_DEVINTERFACE_COMPORT, NULL, NULL, (DIGCF_PRESENT|DIGCF_DEVICEINTERFACE));
  for (i=0; SetupDiEnumDeviceInfo(hDevInfo, i, &DeviceInfoData); i++) {
    HKEY key = SetupDiOpenDevRegKey(hDevInfo, &DeviceInfoData,  DICS_FLAG_GLOBAL, 0, DIREG_DEV, KEY_QUERY_VALUE);
    if ( key ) {
      DWORD type;
      DWORD size, size2;
      type = 0;
      size = max;
      RegQueryValueEx(key, _T("PortName"), NULL, &type , (LPBYTE) tmpbuf, &size);
      size = WideCharToMultiByte(CP_ACP, 0, tmpbuf, wcslen(tmpbuf), ptr, max, NULL, NULL);
#ifdef MAIN
      ptr[size++] = '\n';
#else
      ptr[size++] = '\r';
#endif /* MAIN */
      size2 = sizeof(tmpbuf);
      if (SetupDiGetDeviceRegistryProperty(hDevInfo, &DeviceInfoData, SPDRP_HARDWAREID, &type, (PBYTE)tmpbuf, size2, &size2)) {
	size2 = WideCharToMultiByte(CP_ACP, 0, tmpbuf, wcslen(tmpbuf), &ptr[size], max, NULL, NULL);
	size += size2;
#ifdef MAIN
	ptr[size++] = '\n';
#else
	ptr[size++] = '\r';
#endif /* MAIN */
      }
      ptr[size] = 0;
      ptr += size;
      max -= size;
    }
  }
  SetupDiDestroyDeviceInfoList(hDevInfo);
  return org_max - max;
}

#ifdef MAIN
#include <stdio.h>
char buffer[1024];

void
dump(const char *buf, int size)
{
  int i, j;

  for (i = 0; i < (size + 15) / 16; i ++) {
    for (j = 0; j < 16; j ++) {
      int pos = i * 16 + j;
      if (pos < size) {
	printf("%02X ", (unsigned int) buf[pos]);
      }
    }
    printf("\n");
  }
}

void
main(int argc, char *argv[])
{
  int ret;

  memset(buffer, 0, sizeof buffer);
  ret = MicrobitDevice(buffer, sizeof buffer);
  printf("Microbit Drive:\n%d bytes\n'%s'\n", ret, buffer);

  memset(buffer, 0, sizeof buffer);
  ret = EnumerateComPorts(buffer, sizeof buffer);
  printf("ComPorts:\n%d bytes\n%s\n", ret, buffer);
  printf("Dumps:\n");
  dump(buffer, ret);
}

#endif /* MAIN */
