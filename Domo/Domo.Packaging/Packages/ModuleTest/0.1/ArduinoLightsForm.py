from Domo.Modules import *

from System.Drawing import Point
from System.Threading import Thread, ThreadStart
from System.Windows.Forms import Application, Form, Button

class ArduinoLightsForm(ControllerTriggerModule[ArduinoLightsController]):
	currIndex = 0

	def OnEnable(self):
		self.formThread = None
		self.formThread = Thread(ThreadStart(self.CreateForm))
		self.formThread.Start()
		pass
	
	def OnDisable(self):
		if self.formThread is not None:
			self.formThread.Abort()
		pass

	def CreateForm(self):
		self.form = TestForm()
		self.form.buttonClickedEvent += lambda *a : self.OnTrigger()
		Application.Run(self.form)
		pass

	def OnTrigger(self):
		super(ArduinoLightsForm, self).OnTrigger()

		self.controller.setColor(self.currIndex, self.currIndex, 255, 255, 255)
		self.currIndex += 1

		if self.currIndex > 30:
			self.currIndex = 0
			self.controller.setColor(0, 255, 0, 0, 0)
		pass

class TestForm(Form):
	def __init__(self):
		self.Text = "Test"
		self.Name = "Test"

		self.createControls()
		pass

	def createControls(self):
		self.button = Button()
		self.button.Text = "Test"
		self.button.Location = Point(25, 25)

		self.buttonClickedEvent = self.button.Click

		self.Controls.Add(self.button)
		pass