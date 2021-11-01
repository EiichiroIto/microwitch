'From MIT Squeak 0.9.4 (June 1, 2003) [No updates present.] on 1 November 2021 at 3:15:19 pm'!ExtModule subclass: #HuskyLensModule	instanceVariableNames: ''	classVariableNames: ''	poolDictionaries: ''	category: 'Microwitch-Modules'!!CommandBlockMorph methodsFor: 'private'!uncoloredArgMorphFor: specString	"Answer an argument morph for the given argument specification string."	| code |	code := specString at: 2.	$a = code ifTrue: [^ AttributeArgMorph new choice: 'volume'].	$b = code ifTrue: [^ BooleanArgMorph new].	$c = code ifTrue: [^ ColorArgMorph new showPalette: true].	$C = code ifTrue: [^ LedArgMorph new].	$d = code ifTrue: [^ ExpressionArgMorphWithMenu new numExpression: '0'; menuSelector: #directionMenu].	$D = code ifTrue: [^ ChoiceArgMorph new getOptionsSelector: #ledClockNames].	$e = code ifTrue: [^ EventTitleMorph new].	$f = code ifTrue: [^ ChoiceArgMorph new getOptionsSelector: #mathFunctionNames; choice: 'sqrt'].	$g = code ifTrue: [^ ChoiceArgMorph new getOptionsSelector: #ledSymbolNames].	$H = code ifTrue: [^ ChoiceArgMorph new getOptionsSelector: #sensorNames].	$h = code ifTrue: [^ ChoiceArgMorph new getOptionsSelector: #hookupBooleanSensorNames].	$I = code ifTrue: [^ ChoiceArgMorph new getOptionsSelector: #ledArrowNames].	$i = code ifTrue: [^ ExpressionArgMorphWithMenu new numExpression: '0'; menuSelector: #listIndexMenu].	$j = code ifTrue: [^ ExpressionArgMorphWithMenu new numExpression: '0'; menuSelector: #ioPinMenu].	$k = code ifTrue: [^ ChoiceArgMorph new getOptionsSelector: #neoPixelPinNames].	$L = code ifTrue: [^ ChoiceArgMorph new getOptionsSelector: #listVarMenu].	$l = code ifTrue: [^ ChoiceArgMorph new getOptionsSelector: #ledCharacterNames].	$m = code ifTrue: [^ ChoiceArgMorph new getOptionsSelector: #gestureNames].	$M = code ifTrue: [^ ChoiceArgMorph new getOptionsSelector: #buttonNames].	$n = code ifTrue: [^ ExpressionArgMorph new numExpression: '10'].	$N = code ifTrue: [^ ChoiceArgMorph new getOptionsSelector: #musicNames].	$p = code ifTrue: [^ ExpressionArgMorphWithMenu new numExpression: '0'; menuSelector: #huskyLensProperties].	$s = code ifTrue: [^ ExpressionArgMorph new stringExpression: ''].	$S = code ifTrue: [^ ChoiceArgMorph new getOptionsSelector: #ledShapeNames].	$T = code ifTrue: [^ ChoiceArgMorph new getOptionsSelector: #touchPinNames].	$U = code ifTrue: [^ ChoiceArgMorph new getOptionsSelector: #microphoneNames].	$u = code ifTrue: [^ ExpressionArgMorphWithMenu new numExpression: '0'; menuSelector: #huskyLensAlgorithms].	$v = code ifTrue: [^ ChoiceArgMorph new getOptionsSelector: #varNamesMenu; choice: ''].	$W = code ifTrue: [^ ChoiceArgMorph new getOptionsSelector: #motorDirection].	$x = code ifTrue: [^ ChoiceArgMorph new getOptionsSelector: #shiftDirection].	$y = code ifTrue: [^ ExpressionArgMorphWithMenu new numExpression: '0'; menuSelector: #listIndexForDeleteMenu].	^ ExpressionArgMorph new numExpression: '10'! !!HuskyLensModule methodsFor: 'accessing'!fileContents	^ 'import timefrom microbit import display,i2cheader = bytes([85,170,17])algorithm = NonecacheData = []def select_algorithm(a):    global algorithm    if not (0<=a<=8):	return self    algorithm = a    b = header + bytes([2,45]) + bytes((a,0))    c = checksum_of(b)    b = b + (bytes((c,)))    i2c.write(50,b)    time.sleep_ms(100)    return process_return_data()def get_data():    global cacheData    if algorithm == 0:        cacheData = blocks()    elif algorithm == 1:        cacheData = blocks()    elif algorithm == 3:        cacheData = arrows()    else:        cacheData = request_all()def get_size():    if cacheData is None:        return 0    return len(cacheData)def get_property(p,i):    if cacheData is None:        return -1    if not(0 <= i < len(cacheData)):        return -1    if not(0 <= p < len(cacheData[i])):        return -1    return cacheData[i][p]def request_all():    a = header + bytes([0,32,48])    i2c.write(50,a)    time.sleep_ms(10)    return process_return_data()def blocks():    a = header + bytes([0,33,49])    i2c.write(50,a)    time.sleep_ms(10)    return process_return_data()def arrows():    a = header + bytes([0,34,50])    i2c.write(50,a)    time.sleep_ms(10)    return process_return_data()def checksum_of(a):    c = 0    for b in a:	c = c + b    return c & 255def convert_to_class_object_is_block(b,a):    d = []    for i in b:	c = ("block",i[0],i[1],i[2],i[3],i[4]) if a else ("arrow",i[0],i[1],i[2],i[3],i[4])	d.append(c)    return ddef get_block_or_arrow_command():    a = i2c.read(50,5)    a = a + i2c.read(50,((a[3]) + 1))    b = split_command_to_parts(a)    c = b[3] == 42    return (b[4],c)def knock():    i2c.write(50,(header + bytes([0,44,60])))    time.sleep_ms(10)    return process_return_data()def process_return_data():    g = i2c.read(50,5)    h = i2c.read(50,((g[3]) + 1))    a = split_command_to_parts(g + h)    if (a[3]) == 46:	return [("knockReceived",)]    k = a[4]    if len(k)<2:        return []    f = ((k[1]) * 256) + (k[0])    d = True    j = []    for _ in range(0,f):	k = get_block_or_arrow_command()	d = k[1]	j.append((k[0]))    b = []    for i in j:	k = []	for q in range(0,((len(i)) - 1)+1,2):	    e = i[q]	    c = i[q + 1]	    l = e	    if c > 0:		(e + 255) + c	    k.append(l)	b.append(k)    return convert_to_class_object_is_block(b, d)def split_command_to_parts(a):    g = ((a[0]) * 256) + (a[1])    b = a[2]    f = a[3]    d = a[4]    if f > 0:	e = a[5:(5 + f) - 1+1]    else:	e = []    c = a[5 + f]    return (g,b,f,d,e,c)'! !!MicrobitCode methodsFor: 'private' stamp: 'EiichiroIto 10/29/2021 18:17'!useHuskyLens	modules add: MicrobitModule huskyLens! !!MicrobitCode methodsFor: 'sensor blocks'!getHuskyLensData: aMorph 	stream nextPutAll: HuskyLensModule getHuskyLensData! !!MicrobitCode methodsFor: 'sensor blocks'!huskyLensDataat: aMorph 	| args prop no |	args := aMorph blockArgs.	prop := args first evaluate asNumberNoError asString.	no := args second evaluate asNumberNoError asString.	^ HuskyLensModule huskyLensData: prop at: no! !!MicrobitCode methodsFor: 'sensor blocks'!numberOfHuskyLensData: aMorph 	^ HuskyLensModule numberOfHuskyLensData! !!MicrobitCode methodsFor: 'sensor blocks'!selectHuskyLensAlgorithm: aMorph 	| args no |	self useHuskyLens.	args := aMorph blockArgs.	no := args first evaluate asNumberNoError asString.	stream nextPutAll: (HuskyLensModule selectHuskyLensAlgorithm: no)! !!MicrobitModule class methodsFor: 'instance creation' stamp: 'EiichiroIto 10/29/2021 18:15'!huskyLens	^ HuskyLensModule new! !!HuskyLensModule class methodsFor: 'accessing'!getHuskyLensData	^ self moduleName , '.get_data()'! !!HuskyLensModule class methodsFor: 'accessing'!huskyLensData: aString at: aString2 	^ self moduleName , '.get_property(' , aString , ',' , aString2 , ')'! !!HuskyLensModule class methodsFor: 'accessing'!moduleName	^ 'husky'! !!HuskyLensModule class methodsFor: 'accessing' stamp: 'EiichiroIto 10/29/2021 18:12'!numberOfHuskyLensData	^ self moduleName , '.get_size()'! !!HuskyLensModule class methodsFor: 'accessing' stamp: 'EiichiroIto 10/29/2021 18:12'!selectHuskyLensAlgorithm: aString	^ self moduleName , '.select_algorithm(' , aString , ')'! !!MicrobitSpriteMorph methodsFor: 'list names'!huskyLensAlgorithms	| menu |	menu _ CustomMenu new.	#(('detect faces' 0) ('object tracking' 1) ('line tracking' 3) ) do: [:pair | menu add: '(' asUTF8 , pair second printString , ') ' , pair first localized action: pair second].	^ menu! !!MicrobitSpriteMorph methodsFor: 'list names'!huskyLensProperties	| menu |	menu _ CustomMenu new.	#(('type' 0) ('x' 1) ('y' 2) ('width' 3) ('height' 4) ('x origin' 1) ('y origin' 2) ('x dest' 3) ('y dest' 4) ('id' 5) ) do: [:pair | menu add: '(' asUTF8 , pair second printString , ') ' , pair first localized action: pair second].	^ menu! !!MicrobitSpriteMorph methodsFor: 'sensing ops' stamp: 'EiichiroIto 10/29/2021 16:48'!getHuskyLensData	! !!MicrobitSpriteMorph methodsFor: 'sensing ops' stamp: 'EiichiroIto 10/29/2021 16:49'!huskyLensData: anInteger at: anInteger2	! !!MicrobitSpriteMorph methodsFor: 'sensing ops' stamp: 'EiichiroIto 10/29/2021 16:49'!numberOfHuskyLensData	^ 0! !!MicrobitSpriteMorph methodsFor: 'sensing ops' stamp: 'EiichiroIto 10/29/2021 16:48'!selectHuskyLensAlgorithm: anInteger	! !!MicrobitSpriteMorph class methodsFor: 'block specs'!sensingBlocks	^ #(		'sensing'			('running time'			r	runningTime)			('ticks ms'					r	ticksMs)			('ticks us'					r	ticksUs)			('light'							r	light)			('temperature'				r	temperature)			('%H sensor value'		r	sensorValueOf: 'accelX')			-			('current gesture'		r	currentGesture)			('is gesture %m ?'		b	isGesture: up)			('was gesture %m ?'		b	wasGesture: up)			('gestures'					r	gestures)			-			('calibrate compass'		-	calibrateCompass)			('is calibrated'				b	isCalibrated)			('heading'						r	headingCompass)			('clear calibration'		-	clearCalibration)			('field strength'			r	fieldStrength)			-			('was event %U ?'			b	wasEvent:	LOUD)			('is event %U ?'			b	isEvent:	LOUD)			('set threshold event %U value %n'												-	setThresholdEvent:value:		LOUD	100)			('sound level'				r	soundLevel)			-			('set usonic trig %j echo %j'	-	setUsonicTrig:echo:	1 2)			('usonic distance'		r	usonicDistance)			-			('select HuskyLens algorithm %u'												-	selectHuskyLensAlgorithm: 0)			('get HuskyLens data'	-	getHuskyLensData)			('number of HuskyLens data'												r	numberOfHuskyLensData)			('%p of %n th HuskyLens data'												r	huskyLensData:at:	0 0)	)! !