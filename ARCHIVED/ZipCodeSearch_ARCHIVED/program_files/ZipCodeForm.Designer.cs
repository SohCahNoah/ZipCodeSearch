namespace GUI_test_v3
{
    partial class ZipCodeForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ZipCodeForm));
            search_button = new Button();
            results_slider = new TrackBar();
            slider_label_1 = new Label();
            slider_label_2 = new Label();
            slider_label_3 = new Label();
            slider_label_4 = new Label();
            zip_code_label = new Label();
            user_control_panel = new Panel();
            slider_label = new Label();
            user_entry_panel = new Panel();
            zip_code_entry_box = new TextBox();
            results_container = new Panel();
            results_display_box = new RichTextBox();
            ((System.ComponentModel.ISupportInitialize)results_slider).BeginInit();
            user_control_panel.SuspendLayout();
            user_entry_panel.SuspendLayout();
            results_container.SuspendLayout();
            SuspendLayout();
            // 
            // search_button
            // 
            search_button.Location = new Point(3, 33);
            search_button.Name = "search_button";
            search_button.Size = new Size(137, 23);
            search_button.TabIndex = 0;
            search_button.Text = "Search";
            search_button.UseVisualStyleBackColor = true;
            search_button.Click += Search_button_click;
            // 
            // results_slider
            // 
            results_slider.LargeChange = 3;
            results_slider.Location = new Point(3, 22);
            results_slider.Maximum = 3;
            results_slider.Name = "results_slider";
            results_slider.Size = new Size(104, 45);
            results_slider.SmallChange = 0;
            results_slider.TabIndex = 1;
            results_slider.TabStop = false;
            // 
            // slider_label_1
            // 
            slider_label_1.AutoSize = true;
            slider_label_1.Location = new Point(8, 52);
            slider_label_1.Name = "slider_label_1";
            slider_label_1.Size = new Size(13, 15);
            slider_label_1.TabIndex = 2;
            slider_label_1.Text = "1";
            slider_label_1.Click += Label1_Click;
            // 
            // slider_label_2
            // 
            slider_label_2.AutoSize = true;
            slider_label_2.Location = new Point(34, 52);
            slider_label_2.Name = "slider_label_2";
            slider_label_2.Size = new Size(13, 15);
            slider_label_2.TabIndex = 3;
            slider_label_2.Text = "2";
            slider_label_2.Click += Label2_Click;
            // 
            // slider_label_3
            // 
            slider_label_3.AutoSize = true;
            slider_label_3.Location = new Point(60, 52);
            slider_label_3.Name = "slider_label_3";
            slider_label_3.Size = new Size(13, 15);
            slider_label_3.TabIndex = 4;
            slider_label_3.Text = "3";
            slider_label_3.Click += Label3_Click;
            // 
            // slider_label_4
            // 
            slider_label_4.AutoSize = true;
            slider_label_4.Location = new Point(86, 52);
            slider_label_4.Name = "slider_label_4";
            slider_label_4.Size = new Size(13, 15);
            slider_label_4.TabIndex = 5;
            slider_label_4.Text = "4";
            // 
            // zip_code_label
            // 
            zip_code_label.AutoSize = true;
            zip_code_label.Location = new Point(5, 7);
            zip_code_label.Name = "zip_code_label";
            zip_code_label.Size = new Size(58, 15);
            zip_code_label.TabIndex = 7;
            zip_code_label.Text = "Zip Code:";
            zip_code_label.Click += Label5_Click;
            // 
            // user_control_panel
            // 
            user_control_panel.Controls.Add(slider_label);
            user_control_panel.Controls.Add(slider_label_4);
            user_control_panel.Controls.Add(slider_label_1);
            user_control_panel.Controls.Add(slider_label_2);
            user_control_panel.Controls.Add(slider_label_3);
            user_control_panel.Controls.Add(results_slider);
            user_control_panel.Location = new Point(191, 12);
            user_control_panel.Name = "user_control_panel";
            user_control_panel.Size = new Size(114, 75);
            user_control_panel.TabIndex = 8;
            // 
            // slider_label
            // 
            slider_label.AutoSize = true;
            slider_label.Font = new Font("Segoe UI", 9F, FontStyle.Underline, GraphicsUnit.Point, 0);
            slider_label.Location = new Point(3, 4);
            slider_label.Name = "slider_label";
            slider_label.Size = new Size(108, 15);
            slider_label.TabIndex = 6;
            slider_label.Text = "Number of Results:";
            // 
            // user_entry_panel
            // 
            user_entry_panel.Controls.Add(zip_code_entry_box);
            user_entry_panel.Controls.Add(search_button);
            user_entry_panel.Controls.Add(zip_code_label);
            user_entry_panel.Location = new Point(12, 12);
            user_entry_panel.Name = "user_entry_panel";
            user_entry_panel.Size = new Size(147, 63);
            user_entry_panel.TabIndex = 9;
            // 
            // zip_code_entry_box
            // 
            zip_code_entry_box.Location = new Point(69, 4);
            zip_code_entry_box.MaxLength = 5;
            zip_code_entry_box.Name = "zip_code_entry_box";
            zip_code_entry_box.Size = new Size(71, 23);
            zip_code_entry_box.TabIndex = 8;
            zip_code_entry_box.TextAlign = HorizontalAlignment.Right;
            zip_code_entry_box.TextChanged += TextBox1_TextChanged_1;
            zip_code_entry_box.KeyPress += Zip_code_entry_box_KeyPress;
            // 
            // results_container
            // 
            results_container.Controls.Add(results_display_box);
            results_container.Location = new Point(12, 93);
            results_container.Name = "results_container";
            results_container.Size = new Size(307, 285);
            results_container.TabIndex = 10;
            // 
            // results_display_box
            // 
            results_display_box.BackColor = SystemColors.ControlLightLight;
            results_display_box.Cursor = Cursors.IBeam;
            results_display_box.Dock = DockStyle.Fill;
            results_display_box.Location = new Point(0, 0);
            results_display_box.Name = "results_display_box";
            results_display_box.ReadOnly = true;
            results_display_box.Size = new Size(307, 285);
            results_display_box.TabIndex = 0;
            results_display_box.Text = "Customer Location:\n---\n\nDepot Data:\n---\n\nDistance to Depot:\n --- ";
            // 
            // ZipCodeForm
            // 
            AcceptButton = search_button;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Window;
            ClientSize = new Size(331, 390);
            Controls.Add(results_container);
            Controls.Add(user_entry_panel);
            Controls.Add(user_control_panel);
            ForeColor = SystemColors.ActiveCaptionText;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "ZipCodeForm";
            Text = "ZipCodeSearch";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)results_slider).EndInit();
            user_control_panel.ResumeLayout(false);
            user_control_panel.PerformLayout();
            user_entry_panel.ResumeLayout(false);
            user_entry_panel.PerformLayout();
            results_container.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Button search_button;
        private TrackBar results_slider;
        private Label slider_label_1;
        private Label slider_label_2;
        private Label slider_label_3;
        private Label slider_label_4;
        private Label zip_code_label;
        private Panel user_control_panel;
        private Label slider_label;
        private Panel user_entry_panel;
        private TextBox zip_code_entry_box;
        private Panel results_container;
        private RichTextBox results_display_box;
    }
}
