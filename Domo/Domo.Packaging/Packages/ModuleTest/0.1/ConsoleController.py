from Domo.Modules import *

class ConsoleController(ControllerModule):
	count = 0

	def OnEnable(self):
		pass

	def OnDisable(self):
		pass

	def sendLine(self, line):
		self.count += 1
		Log.Info("ConsoleController:" + line + str(self.count))