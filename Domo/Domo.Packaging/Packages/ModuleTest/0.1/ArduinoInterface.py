from Domo.Modules import *
from Domo.Misc import Config

from System import String
from System.IO.Ports import SerialPort
from System.Threading import Thread, ThreadStart

class ArduinoInterface(HardwareInterfaceModule):
	comPort = "COM3"
	baudRate = 9600
	serial = None
	serialRate = 5		# rate in ms
	isWriting = False
	
	runThread = True
	sendThread = None
	readThread = None

	readBuffer = []

	def OnEnable(self):
		if not Config.ContainsKey("Arduino", "comPort"):
			Config.SetValue(self.comPort, "Arduino", "comPort")
		else:
			self.comPort = Config.GetValue[String]("Arduino", "comPort")

		if not Config.ContainsKey("Arduino", "baudRate"):
			Config.SetValue(self.baudRate, "Arduino", "baudRate")
		else:
			self.baudRate = Config.GetValue[int]("Arduino", "baudRate")

		self.isInitialized = self.openSerial()
		pass

	def OnDisable(self):
		if self.serial is not None:
			self.closeSerial()
		pass

	def SendDataRaw(self, data):
		while self.isWriting:
			continue

		if data is None:
			Log.Error("Received no data to send")

		if self.serial is not None and self.serial.IsOpen:
			self.serial.Write(data, 0, len(data))
		pass

	def ReadDataRaw(self, data):
		bufferSize = len(self.readBuffer)
		
		data.Value = self.readBuffer

		self.hasDataAvailable = False
		self.readBuffer = []

		return bufferSize > 0
		pass

	def openSerial(self):
		if self.serial is not None:
			Log.Error("Serial has already been opened!")
			return False

		if not self.portAvailable(self.comPort):
			Log.Error("Serial port {0} is not available".format(self.comPort))
			return False

		self.serial = SerialPort(self.comPort)
		self.serial.BaudRate = self.baudRate
		self.serial.DtrEnable = True
		
		self.sendThread = Thread(ThreadStart(self.sendDataLoop))
		self.readThread = Thread(ThreadStart(self.readDataLoop))
		self.runThread = True

		self.serial.Open()
		self.sendThread.Start()
		self.readThread.Start()

		Log.Info("Arduino serial has been opened on port {0}".format(self.comPort))
		return True
		pass

	def closeSerial(self):
		if self.serial is None:
			Log.Error("There is no serial to close!")
			return

		self.serial.Close()
		self.runThread = False

		if self.sendThread.IsAlive:
			self.sendThread.Join()
		if self.readThread.IsAlive:
			self.readThread.Join()

		self.serial = None
		self.isInitialized = False
		Log.Info("Arduino serial has been closed")
		pass

	def portAvailable(self, port):
		availablePorts = SerialPort.GetPortNames()
		return port in availablePorts
		pass

	def sendDataLoop(self):
		while self.runThread and self.serial.IsOpen:
			try:
				self.isWriting = True
				self.serial.DiscardOutBuffer()
			except:
				pass
			else:
				self.isWriting = False

			Thread.Sleep(self.serialRate)
			pass
		pass

	def readDataLoop(self):
		while self.runThread and self.serial.IsOpen:
			try:
				if self.serial.BytesToRead > 0:
					buffer = [None] * self.serial.BytesToRead
					self.serial.Read(buffer, 0, len(buffer))
					self.readBuffer.extend(buffer)
			except:
				pass
			else:
				self.hasDataAvailable = True


			Thread.Sleep(self.serialRate)
			pass
		pass