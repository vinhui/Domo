from Domo.Modules import *

from System import Array, Byte

class ArduinoLightsController(ControllerModule):
	def __init__(self):
		ControllerModule.init(self, ArduinoInterface)
		pass

	def OnEnable(self):
		pass

	def OnDisable(self):
		pass

	def setColor(self, led, r, g, b):
		self.setColor(led, led, r, g, b)
		pass

	def setColor(self, startLed, endLed, r, g, b):
		self.SendData(ArduinoData(startLed, endLed, r, g, b))
		pass

class ArduinoData(IRawDataObject):
	startLed = 0
	endLed = 0
	r = 0
	g = 0
	b = 0

	outString = None

	def __init__(self):
		pass

	def __init__(self, start, end, r, g, b):
		self.startLed = start
		self.endLed = end
		self.r = r
		self.g = g
		self.b = b
		pass

	def Read(self, reader):
		self.outString = reader.ReadString()
		pass

	def Write(self, writer):
		bytes = []
		bytes.append(self.startLed)
		bytes.append(self.endLed)
		bytes.append(self.r)
		bytes.append(self.g)
		bytes.append(self.b)

		a = Array[Byte](bytes)
		writer.Write(a)
		pass