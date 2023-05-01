'From MIT Squeak 0.9.4 (June 1, 2003) [No updates present.] on 1 May 2023 at 8:48:23 am'!!ScriptableScratchMorph methodsFor: 'blocks'!addVariableReportersTo: page x: x y: startY	"Add the list block reporters to the given page starting at the given y offset. Answer the new y."	| y stage b line line2 |	y _ startY.	stage _ self ownerThatIsA: ScratchStageMorph.	(stage notNil and: [stage ~= self]) ifTrue: [		stage varNames do: [:vName |			b _ VariableBlockMorph new				commandSpec: vName;				receiver: stage blockReceiver.			page addMorph: (b position: (x + 4)@y).			y _ y + b height + 3].		(self varNames size > 0) ifTrue: [			line _ Morph new.			line				extent: 90@1;				color: Color gray darker darker;				position: x@(y+2).			line2 _ Morph new.			line2				extent: 90@1;				color: Color gray lighter;				position: x@(y+3).			page				addMorph: line;				addMorph: line2.			y _ y + 9]].	self varNames do: [:vName |		b _ VariableBlockMorph new			commandSpec: vName;			receiver: self blockReceiver.		page addMorph: (b position: (x + 4)@y).		y _ y + b height + 3].	^ y! !