from Domo.Modules import *

class ConsoleTrigger(ControllerTriggerModule):
	def __init__(self):
		ControllerTriggerModule.init(self, ConsoleController)
		pass

	def OnEnable(self):
		self.controller.sendLine("This is a test")
		pass

	def OnDisable(self):
		pass