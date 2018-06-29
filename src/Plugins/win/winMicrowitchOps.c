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

#ifdef MAIN
#include <stdio.h>

void
main(int argc, char *argv[])
{
  int ret;
  char buf[1024];

  ret = MicrobitDevice(buf, sizeof buf);
  printf("ret=%d,buf=%s\n", ret, buf);
}

#endif /* MAIN */
