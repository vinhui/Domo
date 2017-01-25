from Domo.Modules import *

class ConsoleTrigger2(ControllerTriggerModule[ConsoleController]):
	def OnEnable(self):
		Delay(lambda *a: self.controller.sendLine("This is another test"), 2000)
		pass

	def OnDisable(self):
		pass