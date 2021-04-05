#!/bin/sh
rm -f *.o MicrobitPlugin
gcc -fPIC -Wall -m32 -pedantic-errors -c MicrowitchPlugin.c unixMicrowitchOps.c
gcc -m32 -shared *.o -o MicrowitchPlugin
rm -f *.o

#gcc -DMAIN -fPIC -Wall -m32 -pedantic-errors unixMicrowitchOps.c
