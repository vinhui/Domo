from Domo.Modules import *

import clr
clr.AddReference("System.Drawing")
clr.AddReference("System.Windows.Forms")

from System.Drawing import Point
from System.Threading import Thread, ThreadStart
from System.Windows.Forms import Application, Form, Button

class FormsInterface(HardwareInterfaceModule):
	def __init__(self):
		self.formThread = None
		pass

	def OnEnable(self):
		self.formThread = Thread(ThreadStart(self.CreateForm))
		self.formThread.Start()
		pass

	def OnDisable(self):
		if self.formThread is not None:
			self.formThread.Abort()
		pass

	def CreateForm(self):
		self.form = TestForm()
		Application.Run(self.form)
		pass

class TestForm(Form):
	def __init__(self):
		self.Text = "Test"
		self.Name = "Test"

		self.createControls()
		pass

	def createControls(self):
		button = Button()
		button.Text = "Test"
		button.Location = Point(25, 25)
		button.Click += self.buttonPressed

		self.Controls.Add(button)
		pass

	def buttonPressed(self, sender, args):
		Log.Info("Button pressed!")
		pass