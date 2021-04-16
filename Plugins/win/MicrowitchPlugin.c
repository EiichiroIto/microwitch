/* Automatically generated from Squeak on (2 January 2009 10:06:55 am) */

#if defined(WIN32) || defined(_WIN32) || defined(Win32)
#define EXPORT __declspec(dllexport)
#else
#define EXPORT
#endif /* WIN32 */

#include "sqVirtualMachine.h"

/* memory access macros */
#define byteAt(i) (*((unsigned char *) (i)))
#define byteAtput(i, val) (*((unsigned char *) (i)) = val)
#define longAt(i) (*((int *) (i)))
#define longAtput(i, val) (*((int *) (i)) = val)

#include "microwitchOps.h"
/* #include <math.h> */
#include <stdlib.h>
#include <string.h>

/*** Variables ***/
struct VirtualMachine* interpreterProxy;
const char *moduleName = "MicrobitPlugin 2 July 2019 (e)";

/*** Functions ***/
EXPORT int primMicrobitDevice(void);
EXPORT int primEnumerateComPorts(void);
EXPORT int setInterpreter(struct VirtualMachine* anInterpreter);

char nameStr[2048];

EXPORT int primMicrobitDevice(void) {
	int i;
	int ret;
	int count;
	int resultOop;
	char* dst;

	ret = MicrobitDevice(nameStr, sizeof nameStr);
	if (ret == 0) {
		interpreterProxy->success(0);
		return 0;
	}
	count = strlen(nameStr);
	resultOop = interpreterProxy->instantiateClassindexableSize(interpreterProxy->classString(), count);
	dst = ((char *) (interpreterProxy->firstIndexableField(resultOop)));
	for (i = 0; i <= (count - 1); i += 1) {
		dst[i] = (nameStr[i]);
	}
	interpreterProxy->popthenPush(1, resultOop);
	return 0;
}

EXPORT int primEnumerateComPorts(void) {
	int i;
	int ret;
	int resultOop;
	char* dst;

	ret = EnumerateComPorts(nameStr, sizeof nameStr);
	if (ret == 0) {
		interpreterProxy->success(0);
		return 0;
	}
	resultOop = interpreterProxy->instantiateClassindexableSize(interpreterProxy->classString(), ret);
	dst = ((char *) (interpreterProxy->firstIndexableField(resultOop)));
	for (i = 0; i <= (ret - 1); i += 1) {
		dst[i] = (nameStr[i]);
	}
	interpreterProxy->popthenPush(1, resultOop);
	return 0;
}

EXPORT int setInterpreter(struct VirtualMachine* anInterpreter) {
	int ok;

	interpreterProxy = anInterpreter;
	ok = interpreterProxy->majorVersion() == VM_PROXY_MAJOR;
	if (ok == 0) {
		return 0;
	}
	ok = interpreterProxy->minorVersion() >= VM_PROXY_MINOR;
	return ok;
}

