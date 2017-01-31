from Domo.Modules import *
from Domo.API import ApiManager, ApiResponse, ApiCodes

from System.Collections.Generic import Dictionary
from System.Drawing import Point, Color, Size, Brush, SolidBrush 
from System.Threading import Thread, ThreadStart
from System.Windows.Forms import (
	Application, 
	Form, 
	DialogResult,
	MethodInvoker,
	Label, 
	Button, 
	NumericUpDown, 
	ColorDialog,
	TrackBar,
	)

class ArduinoLightsForm(ControllerTriggerModule):
	form = None

	def __init__(self):
		pass

	def OnEnable(self):
		ControllerTriggerModule.init(self, ArduinoLightsController)
		ApiManager.RegisterListener("arduinoLights", self.apiListener)
		self.formThread = None
		self.formThread = Thread(ThreadStart(self.CreateForm))
		self.formThread.Start()
		pass
	
	def OnDisable(self):
		if self.form is not None and self.form.Visible:
			self.form.Invoke(MethodInvoker(self.form.Close))

		if self.formThread is not None:
			self.formThread.Abort()

		self.form = None
		pass

	def CreateForm(self):
		self.form = TestForm(self.OnTrigger)
		Application.Run(self.form)
		pass

	def OnTrigger(self):
		super(ArduinoLightsForm, self).OnTrigger()

		if self.form is not None:
			self.controller.setColor(self.form.startLedID.Value, self.form.endLedID.Value, self.form.colorVals[0], self.form.colorVals[1], self.form.colorVals[2])
		pass

	def apiListener(self, request):
		startID = 0
		endID = 255
		r = 0
		g = 0
		b = 0
		
		startID = request.GetArgument[int]("startID", startID)
		endID = request.GetArgument[int]("endID", endID) 
		r = request.GetArgument[int]("r", r)
		g = request.GetArgument[int]("g", g)
		b = request.GetArgument[int]("b", b)

		self.controller.setColor(startID, endID, r, g, b)
		d = {"startID":startID, "endID":endID, "r":r, "g":g, "b":b}
		return ApiResponse.Success(Dictionary[str, object](d))
		pass

class TestForm(Form):
	colorVals = [255] * 3
	callback = None

	def __init__(self, callback):
		self.Text = "Light Controls"
		self.Name = "Test"

		self.callback = callback

		self.createControls()
		pass

	def createControls(self):
		label = Label()
		label.Text = "Start Led: "
		label.Location = Point(10, 25)
		self.Controls.Add(label)

		self.startLedID = NumericUpDown()
		self.startLedID.Value = 0
		self.startLedID.Location = Point(110, 25)
		self.Controls.Add(self.startLedID)

		label = Label()
		label.Text = "End Led: "
		label.Location = Point(10, 50)
		self.Controls.Add(label)

		self.endLedID = NumericUpDown()
		self.endLedID.Value = 30
		self.endLedID.Location = Point(110, 50)
		self.Controls.Add(self.endLedID)

		self.drawColorSliders()
		if self.callback is not None:
			self.callback()
		pass

	def drawColorSliders(self):
		startX = 10
		startY = 75
		offset = 10
		width = 100
		height = 25

		self.bars = [TrackBar(), TrackBar(), TrackBar()]

		for i, b in enumerate(self.bars):
			if i is 0:
				b.BackgroundColorColor = Color.Red
			if i is 1:
				b.BackgroundColorColor = Color.Blue
			if i is 2:
				b.BackgroundColorColor = Color.Green

			b.Size = Size(width, height)
			b.Location = Point(startX, startY + (i * (offset + height)))
			b.Minimum = 0
			b.Maximum = 255
			b.TickFrequency = 1
			b.Value = self.colorVals[i]
			b.Scroll += self.onSliderChanged

			self.Controls.Add(b)
		pass

	def onSliderChanged(self, sender, args):
		self.colorVals[self.bars.index(sender)] = sender.Value
		if self.callback is not None:
			self.callback()
		pass