open System.Drawing
open System.Windows.Forms
open System
open FLogic

let form = new Form(Width = 400, Height = 400, Text = "draw test")
let panel = new FlowLayoutPanel()
form.Controls.Add(panel)

let paint(e : PaintEventArgs) =
    let pen = new Pen(Color.Black);  
    e.Graphics.DrawLine(pen, new PointF(100.0f, 100.0f), new PointF(200.0f, 200.0f))

let button = new Button()
button.Text <- "Click to draw"
button.AutoSize <- true

//button.Click.Add(fun _ -> form.Paint.Add(paint)) // <- does not draw a line on click

//This works
button.Click.Add(fun _ -> form.Paint.Add(paint); form.Invalidate()) 

panel.Controls.Add(button)


let cmd_button = new Button()
cmd_button.Text <- "Click to call"
cmd_button.AutoSize <- true
cmd_button.Click.Add(fun _ -> cmd_button.Text <- Say.hello "Hi") 
panel.Controls.Add(cmd_button)

//form.Paint.Add(paint) //<- here, if uncommented, it will draw a line when the script is run
form.Show()

[<EntryPoint>]
let main argv= 
    printfn "Running F# App"
    System.Windows.Forms.Application.Run(form)
    0