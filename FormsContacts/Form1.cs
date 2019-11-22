using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Questionary;

namespace FormsContacts
{
    public partial class Form1 : Form
    {
        static int currentRow =  -1 ;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            List<QuestionaryModel> questionarylist = DataManager.GetQuestionary();

            RefreshGrid(questionarylist);
        }
        private void KeepLastSelected()
        {
             if (currentRow > -1 )
            {
                dataGridView1.ClearSelection();
                dataGridView1.Rows[0].Selected = false;
                dataGridView1.Rows[0].DefaultCellStyle.BackColor = Color.White;
                dataGridView1.CurrentCell = dataGridView1.Rows[currentRow].Cells[0];
                dataGridView1.Rows[currentRow].Selected = true;
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) { return; }
          
            btnDelete.Visible = true;
          
            btnSend.Text = "Actualizar" ;
            
            currentRow = e.RowIndex;

            txtId.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtQuestion.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtAnswer.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();


        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtQuestion.Text) || string.IsNullOrEmpty(txtAnswer.Text))
            {
                MessageBox.Show("Pregunta y respuesta son obligatorios", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            QuestionaryModel quest= new QuestionaryModel()
            {
                Id = string.IsNullOrEmpty(txtId.Text) ? 0 :  Convert.ToInt32(txtId.Text ),
                Question = txtQuestion.Text,
                Answer = txtAnswer.Text,
            };

            if (DataManager.InsertOrUpdate(quest) <= 0)
            {
                MessageBox.Show("Hubo un error al registrar, verifica", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("registro agregado de forma correcta", "Nuevo contacto", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            List<QuestionaryModel> questionarylist = DataManager.GetQuestionary();
         
            RefreshGrid(questionarylist);

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtId.Text) ){ 

            var confirmResult = MessageBox.Show("Are you sure to delete item " + txtId.Text + " ?? ",
                                      "Confirm Delete!!",
                                      MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                    currentRow = 0;

                    int id = Convert.ToInt32(txtId.Text); 
                DataManager.Delete(id);
            }

                List<QuestionaryModel> questionarylist = DataManager.GetQuestionary();

                RefreshGrid(questionarylist);
            }
        }
        private void RefreshGrid(List<QuestionaryModel> questionarylist)
        {
            btnDelete.Visible = false;
            btnSend.Text = "Enviar";
            txtId.Text = "";
            txtQuestion.Text = "";
            txtAnswer.Text = "";
            dataGridView1.DataSource = questionarylist;
            KeepLastSelected();
        }
    }
}
