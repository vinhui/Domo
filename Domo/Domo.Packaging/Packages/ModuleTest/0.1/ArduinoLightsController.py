from Domo.Modules import *

from System import Array, Byte

class ArduinoLightsController(ControllerModule[ArduinoInterface]):
	def OnEnable(self):
		pass

	def OnDisable(self):
		pass

	def setColor(self, led, r, g, b):
		self.setColor(led, led, r, g, b)
		pass

	def setColor(self, startLed, endLed, r, g, b):
		bytes = []
		bytes.append(startLed)
		bytes.append(endLed)
		bytes.append(r)
		bytes.append(g)
		bytes.append(b)

		self.SendData(Array[Byte](bytes))
		pass