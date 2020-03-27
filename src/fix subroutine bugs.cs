'From MIT Squeak 0.9.4 (June 1, 2003) [No updates present.] on 27 March 2020 at 9:50:09 am'!!HatBlockMorph methodsFor: 'microwitch' stamp: 'EiichiroIto 3/26/2020 20:59'!varNames	| stage |	(stage := self receiver ownerThatIsA: ScratchStageMorph) ifNil: [^ #()].	^ stage varNames! !!MicrobitCode methodsFor: 'code generator'!generateGlobalVars: aCollection	| list |	list := aCollection asOrderedCollection.	stream nextPutAll: 'global ';	 nextPutAll: list first.	list allButFirst do: [:each | stream nextPut: $,;		 nextPutAll: each].	stream nextPutAll: newlineString! !!MicrobitCode methodsFor: 'code generator'!generateSubroutineHeader: aString 	aString = 'Scratch-StartClicked' ifTrue: [^ self].	stream	 nextPutAll: newlineString;	 nextPutAll: 'def ';	 nextPutAll: (self subNameFor: aString);	 nextPutAll: '():';	 nextPutAll: newlineString.! !!MicrobitCode methodsFor: 'control blocks'!callSubroutine: aMorph 	| subName |	subName := self subNameFor: aMorph blockArgs first eventName.	stream nextPutAll: subName.	stream nextPutAll: '()'! !!MicrobitCode methodsFor: 'private' stamp: 'EiichiroIto 3/26/2020 20:27'!subNameFor: aString	^ 'sub_', aString replaceAll: Character space with: $_! !!ScratchCodeGenerator methodsFor: 'code generator'!emitEventHatBlock: aMorph indent: indent 	| str indent2 |	str := aMorph eventName.	str isEmpty ifTrue: [^ self].	target generateSubroutineHeader: str.	aMorph nextBlock		ifNotNil: 			[indent2 := indent + (target indentLevelFor: str).			self emitGlobalVars: aMorph varNames indent: indent2.			self emitCode: aMorph nextBlock indent: indent2]! !!ScratchCodeGenerator methodsFor: 'code generator'!emitGlobalVars: aCollection indent: indent 	aCollection isEmpty ifTrue: [^ self].	indent = 0 ifTrue: [^ self].	target generateIndent: indent;	 generateGlobalVars: aCollection! !!ScratchFrameMorph methodsFor: 'microwitch'!hasDuplicateHatBlocks	| set eventName |	set := Set new.	self currentHatBlocks do: 		[:each | 		eventName := each eventName.		(set includes: eventName)			ifTrue: [^ true].		set add: eventName].	^ false! !!ScratchFrameMorph methodsFor: 'microwitch'!sendMicrobit	| path script |	self hasDuplicateHatBlocks		ifTrue: [^ DialogBoxMorph inform: 'has duplicate hat blocks.' localized].	mpboard isConnecting		ifTrue: 			[script := MicrobitCode new newlineCR; pythonScriptFrom: self currentHatBlocks stageMorph: workPane.			self stopAll.			mpboard write: script fileNamed: 'main.py'.			mpboard reboot]		ifFalse: 			[path := self hexCodePath.			path ifNil: [^ self].			(DialogBoxMorph ask: 'send firmware to micro:bit?')				ifFalse: [^ self].			self writeHexCodeAs: path]! !!ScratchFrameMorph methodsFor: 'microwitch'!writeHexCode	| path |	self hasDuplicateHatBlocks		ifTrue: [^ DialogBoxMorph inform: 'has duplicate hat blocks.' localized].	path := self hexCodePathByUser.	path ifNil: [^ self].	self writeHexCodeAs: path.! !!ScratchFrameMorph methodsFor: 'microwitch'!writePythonProgram	| path |	self hasDuplicateHatBlocks		ifTrue: [^ DialogBoxMorph inform: 'has duplicate hat blocks.' localized].	path := self programPathByUser.	path ifNil: [^ self].	self writePythonProgramAs: path.! !!ScriptableScratchMorph methodsFor: 'private'!hatBlocks	| list v |	list := blocksBin submorphs select: [:m | m isKindOf: HatBlockMorph].	v := list detect: [:each | each eventName = 'Scratch-StartClicked']				ifNone: [].	v		ifNotNil: 			[list := list copyWithout: v.			list := list copyWith: v].	^ list! !