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
  TCHAR tmpbuf[128];

  SP_DEVINFO_DATA DeviceInfoData = {sizeof(SP_DEVINFO_DATA)};
  HDEVINFO hDevInfo = SetupDiGetClassDevs(&GUID_DEVINTERFACE_COMPORT, NULL, NULL, (DIGCF_PRESENT|DIGCF_DEVICEINTERFACE));
  for (i=0; SetupDiEnumDeviceInfo(hDevInfo, i, &DeviceInfoData); i++) {
    HKEY key = SetupDiOpenDevRegKey(hDevInfo, &DeviceInfoData,  DICS_FLAG_GLOBAL, 0, DIREG_DEV, KEY_QUERY_VALUE);
    if ( key ) {
      DWORD type = 0;
      DWORD size = max;
      RegQueryValueEx(key, _T("PortName"), NULL, &type , (LPBYTE) tmpbuf, &size);
      size = WideCharToMultiByte(CP_ACP, 0, tmpbuf, wcslen(tmpbuf), ptr, max, NULL, NULL);
      ptr[size++] = '\r';
#ifdef MAIN
      ptr[size++] = '\n';
#endif /* MAIN */
      ptr[size] = 0;
      ptr += size;
      max -= size;
    }
  }
  return org_max - max;
}

#ifdef MAIN
#include <stdio.h>

void
main(int argc, char *argv[])
{
  int ret;
  char buf[1024];

  ret = MicrobitDevice(buf, sizeof buf);
  printf("ret=%d,buf=%s\n", ret, buf);

  ret = EnumerateComPort(buffer, sizeof buffer);
  printf("size=%d\n", ret);
  printf("ComPorts=<%s>\n", buffer);
}

#endif /* MAIN */
