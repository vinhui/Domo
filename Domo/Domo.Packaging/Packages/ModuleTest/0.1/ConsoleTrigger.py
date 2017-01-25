from Domo.Modules import *

class ConsoleTrigger(ControllerTriggerModule[ConsoleController]):
	def OnEnable(self):
		self.controller.sendLine("This is a test")
		pass

	def OnDisable(self):
		pass