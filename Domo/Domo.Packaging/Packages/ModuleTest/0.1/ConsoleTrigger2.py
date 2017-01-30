from Domo.Modules import *

class ConsoleTrigger2(ControllerTriggerModule):
	def __init__(self):
		ControllerTriggerModule.init(self, ConsoleController)
		pass

	def OnEnable(self):
		Delay(lambda *a: self.controller.sendLine("This is another test"), 2000)
		pass

	def OnDisable(self):
		pass