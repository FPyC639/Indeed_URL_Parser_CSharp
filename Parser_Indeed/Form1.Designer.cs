namespace Parser_Indeed
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private Label i_text;
        private FlowLayoutPanel flow;
        private TextBox textbox;
        private Button submitButton;
        private List<string> submissions = new List<string>();
        private Button filesavesubmitButton;


        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "Indeed URL Parser";

            // Create and configure the FlowLayoutPanel
            this.flow= new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                FlowDirection = FlowDirection.TopDown, // Controls will be added vertically
                WrapContents = false, // Prevents wrapping, making it a single column layout
            };

            // Initialize the label
            this.i_text = new Label
            {
                AutoSize = true,
                Font = new Font("Calibri", 12.6F, FontStyle.Regular, GraphicsUnit.Point),
                Text = "Insert Link for processing:",
                Margin = new Padding(20) // Adds some padding around the label
            };

            // Add the label to the FlowLayoutPanel
            this.flow.Controls.Add(this.i_text);

            // Add another control as an example
            this.textbox = new TextBox
            {
                Width = 300, // Fixed width
                Margin = new Padding(20, 0, 20, 20) // Custom margins for better spacing
                
            };

            // Add the TextBox to the FlowLayoutPanel
            this.flow.Controls.Add(this.textbox);
            
            // Submit Button
            this.submitButton = new Button
            {
                Text = "Submit",
                Width = 100,
                Location = new Point(320, 20)
            };
            this.submitButton.Click += SubmitButton_Click;
            this.Controls.Add(this.submitButton);
            this.filesavesubmitButton = new Button
            {
                Text = "Save File",
                Width = 100,
                Location = new Point(440, 20)
            };
            this.filesavesubmitButton.Click += NotifyAndPrepareFile;
            this.Controls.Add(this.filesavesubmitButton);

            // Add the FlowLayoutPanel to the form
            this.Controls.Add(this.flow);

            
            
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
                string inputText = this.textbox.Text;
                string outputText = TransformUrl(inputText);
                submissions.Add(outputText);

                // Clear the TextBox
                this.textbox.Text = string.Empty;

                // Add a label with the input text
                Label label = new Label
                {
                    Text = outputText,
                    AutoSize = true,
                    Margin = new Padding(20)
                };
                this.flow.Controls.Add(label);
        }

        private void NotifyAndPrepareFile(object sender, EventArgs e)
        {
            MessageBox.Show("Adding them to a plain text file for download.");
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                saveFileDialog.Title = "Save an Text File";
                saveFileDialog.CheckPathExists = true;
                saveFileDialog.FileName = "MyTextFile.txt"; // Default file name

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Get the file path from the dialog
                    string filePath = saveFileDialog.FileName;

                    // Assuming 'submissions' is a List<string> containing data to be saved
                    File.WriteAllLines(filePath, submissions);

                    MessageBox.Show($"File saved successfully at: {filePath}", "File Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Save operation cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }
        public static string TransformUrl(string inputUrl)
        {
            try
            {
                // Parse the URL to get the query string
                var uri = new Uri(inputUrl);
                var queryString = uri.Query.TrimStart('?');

                // Extract the job key (vjk) from the query string
                string jobKey = null;
                var queryParts = queryString.Split('&');
                foreach (var part in queryParts)
                {
                    var keyValue = part.Split('=');
                    if (keyValue[0] == "vjk" && keyValue.Length > 1)
                    {
                        jobKey = keyValue[1];
                        break;
                    }
                }

                if (string.IsNullOrEmpty(jobKey))
                {
                    return "No job key found in URL";
                }

                // Construct the new URL
                return $"https://www.indeed.com/viewjob?jk={jobKey}";
            }
            catch (Exception ex)
            {
                return $"Error processing URL: {ex.Message}";
            }
        }
    }


    #endregion
}