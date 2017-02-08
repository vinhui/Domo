from Domo.Modules import *

from System import Array, Byte

class ArduinoLightsController(ControllerModule):
	def __init__(self):
		ControllerModule.init(self, ArduinoInterface)
		pass

	def OnEnable(self):
		pass

	def OnDisable(self):
		self.SendData(ArduinoData(0, 255, 0, 0, 0))
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
		self.startLed = clamp(start, 0, 255)
		self.endLed = clamp(end, 0, 255)
		self.r = clamp(r, 0, 255)
		self.g = clamp(g, 0, 255)
		self.b = clamp(b, 0, 255)
		pass

	def Read(self, reader):
		self.outString = reader.ReadString()
		pass

	def Write(self, writer):
		bytes = []
		bytes.append(clamp(self.startLed, 0, 255))
		bytes.append(clamp(self.endLed, 0, 255))
		bytes.append(clamp(self.r, 0, 255))
		bytes.append(clamp(self.g, 0, 255))
		bytes.append(clamp(self.b, 0, 255))

		a = Array[Byte](bytes)
		writer.Write(a)
		pass