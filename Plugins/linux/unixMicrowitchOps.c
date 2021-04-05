#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include "microwitchOps.h"

#define BUF 512

int
MicrobitDevice(char *dst, int max)
{
  FILE	*fp;
  char	buf[BUF], *p, c, *volume;
  char	*cmdline = "mount";
  int count, len;

  if ((fp = popen(cmdline,"r")) == NULL) {
    return 0;
  }
  while(fgets(buf, BUF, fp) != NULL) {
    p = buf;
    count = 0;
    volume = NULL;
    while ((c = *p++) != '\0') {
      if (c == ' ') {
	if (count == 1) {
	  volume = p;
	} else if (count == 2) {
	  *--p = '\0';
	  break;
	}
	count ++;
      }
    }
    if (volume) {
      len = strlen(volume);
      if (len > 8) {
	if (strcmp(&volume[len-8], "MICROBIT") == 0) {
	  (void) pclose(fp);
	  strncpy(dst, volume, max);
	  return 1;
	}
      }
    }
  }
  (void) pclose(fp);
  return 0;
}

#ifdef MAIN

int
main(int argc, char *argv[])
{
  int ret;
  char buf[1024];

  ret = MicrobitDevice(buf, sizeof buf);
  printf("ret=%d,buf=%s\n", ret, buf);
}

#endif /* MAIN */
