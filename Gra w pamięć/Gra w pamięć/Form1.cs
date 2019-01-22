using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.IO;

namespace Gra_w_pamięć
{
    public partial class Form1 : Form
    {
        public static int ID = 0;
        string scieżka=System.IO.Directory.GetCurrentDirectory();
        PictureBox[] picturebox = new PictureBox[20];
        PictureBox[] życia = new PictureBox[2];
        PictureBox zasłona = new PictureBox();
        public static Obiekt[] obiekt_ = new Obiekt[20];
        int runda=1;
        int część_rundy = 1;
        Image tło;
        Image[] przycisk_informacje= new Image[2];
        Image[,] przycisk_playpause = new Image[2,2];
        Image[] przycisk_start = new Image[2];
        Image pixel;
        Image[,] obraz_obiektu = new Image[2,10];
        Timer stoper = new Timer();
        Label[] wyniki = new Label[11];
        Label lbl_rundy = new Label();
        PictureBox Przycisk_Start = new PictureBox();
        PictureBox Przycisk_Informacje = new PictureBox();
        PictureBox Przycisk_PlayPause= new PictureBox();
        //PictureBox Przycisk_Statystyki = new PictureBox();
        int informacje=0, statystyki=0;
        bool wybrano = false, play=false;
        public static int[,,] wykozystane_pola=new int[3,20,2];
        public static int kolejka = 0;
        int ile_obiektow = 0;
        System.Windows.Forms.ToolTip ToolTip_Przycisk_PlayPause = new System.Windows.Forms.ToolTip();
        System.Windows.Forms.ToolTip ToolTip_Przycisk_Informacje = new System.Windows.Forms.ToolTip();
        System.Windows.Forms.ToolTip ToolTip_Przycisk_Statystyki = new System.Windows.Forms.ToolTip();
        int poprawne = 0;
        int nr_gry = 0;
        string[] linia = new string[10000];
        int[] punkty = new int[10000];
        int mouseon = 0, nr_grafik=1;

        public Form1()
        {
            InitializeComponent();
            tło = Image.FromFile(scieżka + @"\img\Tło.png");
            przycisk_informacje[0] = Image.FromFile(scieżka + @"/img/mouse_leave/informacje.png");
            przycisk_informacje[1] = Image.FromFile(scieżka + @"/img/mouse_hover/informacje.png");
            przycisk_start[0] = Image.FromFile(scieżka + @"/img/mouse_leave/Start.png");
            przycisk_start[1] = Image.FromFile(scieżka + @"/img/mouse_hover/Start.png");
            przycisk_playpause[0, 0] = Image.FromFile(scieżka + @"/img/mouse_leave/Pause.png");
            przycisk_playpause[0, 1] = Image.FromFile(scieżka + @"/img/mouse_leave/Play.png");
            przycisk_playpause[1, 0] = Image.FromFile(scieżka + @"/img/mouse_hover/Pause.png");
            przycisk_playpause[1, 1] = Image.FromFile(scieżka + @"/img/mouse_hover/Play.png");
            pixel = Image.FromFile(scieżka + @"/img/pixOn.png");
            for (int i = 1; i < 2; i++)
            {
                obraz_obiektu[0, i] = Image.FromFile(scieżka + @"/img/" + i + "/Obiekt.png");
                obraz_obiektu[1, i] = Image.FromFile(scieżka + @"/img/" + i + "/Obiektpowiększony.png");
            }
            nr_gry = File.ReadLines(scieżka + @"/txt/statystyki.txt").Count() + 1;
            for (int i = 0; i < 3;i++ )
            {
                for(int j=0;j<20;j++)
                {
                    wykozystane_pola[i, j, 0] = -1;
                    wykozystane_pola[i, j, 1] = -1;
                }
            }
            this.Controls.Add(zasłona);
            zasłona.Visible = false;
            zasłona.Size = new System.Drawing.Size(80,80);
            zasłona.BackColor = Color.Transparent;
            Przycisk_Informacje.BackColor = Color.Transparent;
            Przycisk_PlayPause.BackColor = Color.Transparent;
            //Przycisk_Statystyki.BackColor = Color.Transparent;
            this.Controls.Add(lbl_rundy);
            lbl_rundy.Size = new System.Drawing.Size(150,30);
            lbl_rundy.Location = new System.Drawing.Point(415,735);
            lbl_rundy.BackColor = Color.Transparent;
            lbl_rundy.Font = new System.Drawing.Font("Times New Roman", 16);
            this.stoper.Tick += new System.EventHandler(this.stoper_tick);
            this.BackgroundImage = tło;
            this.Controls.Add(Przycisk_Start);
            Przycisk_Start.Size = new System.Drawing.Size(100,100);
            Przycisk_Start.Location = new System.Drawing.Point(580,340);
            Przycisk_Start.Click += new System.EventHandler(this.Przycisk_Start_Click);
            Przycisk_Start.MouseHover += new System.EventHandler(this.przycisk_hover);
            Przycisk_Start.MouseLeave += new System.EventHandler(this.przycisk_leave);
            Przycisk_Start.BackColor = Color.Transparent;
            Przycisk_Start.Image = przycisk_start[0];
            Przycisk_Start.Name = "start";
            Font Czcionka = new System.Drawing.Font("Times New Roman", 7.5f);
            this.Controls.Add(Przycisk_PlayPause);
            this.Controls.Add(Przycisk_Informacje);
            Przycisk_PlayPause.Padding = Padding.Empty;
            Przycisk_PlayPause.Font = Czcionka;
            Przycisk_PlayPause.Visible = false;
            //this.Controls.Add(Przycisk_Statystyki);
            //Przycisk_Statystyki.Padding = Padding.Empty;
            //Przycisk_Statystyki.Font = Czcionka;
            //Przycisk_Statystyki.Visible = false;
            Przycisk_Informacje.Visible = false;
            ToolTip_Przycisk_Informacje.SetToolTip(this.Przycisk_Informacje, "Informacje");
            ToolTip_Przycisk_PlayPause.SetToolTip(this.Przycisk_PlayPause, "Pause");
            //ToolTip_Przycisk_Statystyki.SetToolTip(this.Przycisk_Statystyki, "Statystyki");
            Przycisk_PlayPause.Image = przycisk_playpause[0,0];
            for (int i = 0; i < 2; i++)
            {
                życia[i] = new PictureBox();
                this.Controls.Add(życia[i]);
                życia[i].BackColor = Color.Transparent;
                życia[i].Location = new System.Drawing.Point(680+40*i, 735);
                życia[i].Size = new System.Drawing.Size(40, 40);
            }
        }

        //wykonuje się po kliknięciu przycisku start
        private void Przycisk_Start_Click(object sender, EventArgs e)
        {
            Przycisk_Informacje.Visible = true;
            this.Controls.Remove(this.Przycisk_Start);
            Przycisk_Informacje.Location = new System.Drawing.Point(585, 735);
            Przycisk_Informacje.Click += new System.EventHandler(this.Przycisk_Informacje_Click);
            Przycisk_Informacje.MouseHover += new System.EventHandler(this.przycisk_hover);
            Przycisk_Informacje.MouseLeave += new System.EventHandler(this.przycisk_leave);
            Przycisk_Informacje.Size = new System.Drawing.Size(40,40);
            Przycisk_Informacje.Image = przycisk_informacje[0];
            Przycisk_Informacje.Name = "informacje";
            Przycisk_PlayPause.Size = new System.Drawing.Size(40, 40);
            Przycisk_PlayPause.Location = new System.Drawing.Point(635, 735);
            Przycisk_PlayPause.Click += new System.EventHandler(this.Przycisk_PlayPause_Click);
            Przycisk_PlayPause.MouseHover += new System.EventHandler(this.przycisk_hover);
            Przycisk_PlayPause.MouseLeave += new System.EventHandler(this.przycisk_leave);
            Przycisk_PlayPause.Name = "pause";
            //Przycisk_Statystyki.Size = new System.Drawing.Size(40, 40);
            //Przycisk_Statystyki.Location = new System.Drawing.Point(535, 735);
            //Przycisk_Statystyki.Click += new System.EventHandler(this.Przycisk_Statystyki_Click);
            //Przycisk_Statystyki.MouseHover += new System.EventHandler(this.przycisk_hover);
            //Przycisk_Statystyki.MouseLeave += new System.EventHandler(this.przycisk_leave);
            //Przycisk_Statystyki.Image = Image.FromFile(scieżka + @"/img/mouse_leave/statystyki.png");
            //Przycisk_Statystyki.Name = "statystyki";
            play = true;
            /*
            int a = File.ReadLines(scieżka + @"/txt/statystyki.txt").Count();
            System.IO.StreamReader plik1 = new System.IO.StreamReader(scieżka + @"/txt/statystyki.txt", System.Text.Encoding.GetEncoding("ISO-8859-2"));
            for (int i = 0; i < a; i++)
            {
                linia[i] = plik1.ReadLine();
            }
            plik1.Close();
            TextWriter czyszczenie = new StreamWriter(scieżka + @"/txt/statystyki.txt", true);
            czyszczenie.Dispose();
            czyszczenie.Close();
            TextWriter plik2 = new StreamWriter(scieżka + @"/txt/statystyki.txt", false);
            for (int i = 0; i < a; i++)
            {
                plik2.WriteLine(linia[i]);
            }
            DateTime dzisiaj = DateTime.Now;
            string data = dzisiaj.Day + "." + dzisiaj.Month + "." + dzisiaj.Year;
            plik2.WriteLine(nr_gry + ";" + data + ";" + runda);
            plik2.Close();
             */
            nowa_runda();
        }

        private void przycisk_hover(object sender, EventArgs e)
        {
            PictureBox picturebox = sender as PictureBox;
            switch(picturebox.Name)
            {
                case "informacje":
                    picturebox.Image = przycisk_informacje[1];
                    break;
                case "play":
                    picturebox.Image = przycisk_playpause[1,1];
                    break;
                case "pause":
                    picturebox.Image = przycisk_playpause[1,0];
                    break;
                case "start":
                    picturebox.Image = przycisk_start[1];
                    break;
            }
        }

        private void przycisk_leave(object sender, EventArgs e)
        {
            PictureBox picturebox = sender as PictureBox;
            switch (picturebox.Name)
            {
                case "informacje":
                    picturebox.Image = przycisk_informacje[0];
                    break;
                case "play":
                    picturebox.Image = przycisk_playpause[0, 1];
                    break;
                case "pause":
                    picturebox.Image = przycisk_playpause[0, 0];
                    break;
                case "start":
                    picturebox.Image = przycisk_start[0];
                    break;
            }
        }

        /*private void Przycisk_Statystyki_Click(object sender, EventArgs e)
        {
            if(statystyki==0)
            {
                statystyki = 1;
                stoper.Stop();
                for (int i = ID; i > 0; i--)
                {
                    obiekt_[i] = null;
                    picturebox[i - 1].Visible = false;
                }
                System.IO.StreamReader plik1 = new System.IO.StreamReader(scieżka + @"/txt/statystyki.txt", System.Text.Encoding.GetEncoding("ISO-8859-2"));
                for (int i = 0; i < nr_gry; i++)
                {
                    linia[i] = plik1.ReadLine();
                    string[] a = linia[i].Split(';');
                    punkty[i] = Convert.ToInt32(a[2]);
                }
                plik1.Close();
                //sortowanie
                int MAX=0;
                string str_z_MAXEM = null;
                for(int i=0;i<nr_gry;i++)
                {
                    for (int j = 0; j < nr_gry-1; j++)
                    {
                        if (punkty[j] < punkty[j + 1])
                        {
                            MAX = punkty[j];
                            punkty[j] = punkty[j + 1];
                            punkty[j + 1] = MAX;
                            ///////////////////////////
                            str_z_MAXEM = linia[j];
                            linia[j] = linia[j + 1];
                            linia[j + 1] = str_z_MAXEM;
                        }
                    }
                }
                wyniki[0] = new Label();
                this.Controls.Add(wyniki[0]);
                wyniki[0].Font = new System.Drawing.Font("Times New Roman", 16);
                wyniki[0].Size = new System.Drawing.Size(200, 30);
                wyniki[0].Location = new System.Drawing.Point(530, 100);
                wyniki[0].Text = "Najlepsze wyniki: ";
                wyniki[0].BackColor = Color.Transparent;
                for(int i=1;i<11&&i<nr_gry+1;i++)
                {
                    wyniki[i] = new Label();
                    this.Controls.Add(wyniki[i]);
                    wyniki[i].BackColor = Color.Transparent;
                    wyniki[i].Font = new System.Drawing.Font("Times New Roman", 16);
                    wyniki[i].Size = new System.Drawing.Size(200,30);
                    wyniki[i].Location = new System.Drawing.Point(530,130+30*i);
                    string[] a = linia[i-1].Split(';');
                    wyniki[i].Text = a[2] + " (" + a[1] + ")";
                }
                ToolTip_Przycisk_Statystyki.SetToolTip(this.Przycisk_Statystyki, "Powrót do gry");
                Przycisk_Informacje.Visible = false;
            }
            else
            {
                ToolTip_Przycisk_Statystyki.SetToolTip(this.Przycisk_Statystyki, "Statystyki");
                for (int i = 0; i < 11; i++)
                {
                    this.Controls.Remove(wyniki[i]);
                }
                Przycisk_Informacje.Visible = true;
                Przycisk_Statystyki.Visible = false;
                statystyki = 0;
                część_rundy = 1;
                nowa_runda();
            }
        }
        */

        //wykonuje się przy kliknięciu przycisku play/pause
        private void Przycisk_PlayPause_Click(object sender, EventArgs e)
        {
            if(play==true)
            {
                play = false;
                Przycisk_PlayPause.Image = przycisk_playpause[0,1];
                ToolTip_Przycisk_PlayPause.SetToolTip(this.Przycisk_PlayPause, "Play");
                Przycisk_PlayPause.Name = "play";
                stoper.Stop();
            }
            else
            {
                play = true;
                Przycisk_PlayPause.Image = przycisk_playpause[0,0];
                ToolTip_Przycisk_PlayPause.SetToolTip(this.Przycisk_PlayPause, "Pause");
                Przycisk_PlayPause.Name = "pause";
                stoper.Start(); 
            }
        }

        //wykonuje się po kliknięciu przycisku informacje
        private void Przycisk_Informacje_Click(object sender, EventArgs e)
        {
            if(informacje==0)
            {
                informacje = 1;
                stoper.Stop();
                for (int i = ID; i > 0; i--)
                {
                    obiekt_[i] = null;
                    picturebox[i-1].Visible = false;
                }
                ToolTip_Przycisk_Informacje.SetToolTip(this.Przycisk_Informacje, "Powrót do gry");
            }
            else
            {
                Przycisk_Informacje.Image = przycisk_informacje[0];
                ToolTip_Przycisk_Informacje.SetToolTip(this.Przycisk_Informacje, "Informacje");
                informacje = 0;
                część_rundy = 1;
                nowa_runda();
            }
        }

        /*
        void update_statystyk()
        {
            int a=File.ReadLines(scieżka + @"/txt/statystyki.txt").Count();
            System.IO.StreamReader plik1 = new System.IO.StreamReader(scieżka + @"/txt/statystyki.txt", System.Text.Encoding.GetEncoding("ISO-8859-2"));
            for (int i = 0; i < a; i++)
            {
                linia[i] = plik1.ReadLine();    
            }
            plik1.Close();
            TextWriter czyszczenie = new StreamWriter(scieżka + @"/txt/statystyki.txt", false);
            czyszczenie.Dispose();
            czyszczenie.Close();
            TextWriter plik2 = new StreamWriter(scieżka + @"/txt/statystyki.txt", true);
            for (int i = 0; i < a-1; i++)
            {
                plik2.WriteLine(linia[i]);    
            }
            DateTime dzisiaj = DateTime.Now;
            string data = dzisiaj.Day + "." + dzisiaj.Month + "." + dzisiaj.Year;
            plik2.WriteLine(nr_gry+";"+data+";"+runda);
            plik2.Close();
        }
        */

        private void click(object sender, EventArgs e)
        {
            if(wybrano==false)
            {
                PictureBox picturebox = sender as PictureBox;
                Point pozycja = picturebox.Location;
                int numer = Convert.ToInt32(picturebox.Name);
                if (mouseon == 1)
                {
                    mouseon = 0;
                    zasłona.Location = new System.Drawing.Point(pozycja.X + 10, pozycja.Y + 10);
                }
                else
                {
                    zasłona.Location = picturebox.Location;
                }
                if (numer == ID - 1 && wybrano == false && play == true)
                {
                    poprawne++;
                    wybrano = true;
                    SoundPlayer dźwięk_poprawny = new SoundPlayer(scieżka + @"/wav/yes.wav");
                    dźwięk_poprawny.Play();
                    zasłona.Image = Image.FromFile(scieżka + @"/img/dobry.png");
                    picturebox.Visible = false;
                    zasłona.Visible = true;
                    timer1.Interval = 3000;
                    timer1.Start();
                    Przycisk_PlayPause.Visible = false;
                }
                else if (wybrano == false && play == true && część_rundy == 3)
                {
                    SoundPlayer dźwięk_niepoprawny = new SoundPlayer(scieżka + @"/wav/no.wav");
                    dźwięk_niepoprawny.Play();
                    wybrano = true;
                    picturebox.Visible = false;
                    zasłona.Image = Image.FromFile(scieżka + @"/img/zły.png");
                    zasłona.Visible = true;
                    //obiekt_[ID - 1].Picturebox.Image = Image.FromFile(scieżka + @"/img/Obiektzły.png"); ten, który miał byc kliknięty
                    timer1.Interval = 3000;
                    timer1.Start();
                    if (poprawne >= 0 && runda != 1)
                    {
                        poprawne--;
                    }
                    else if (runda == 1 && poprawne > 0)
                    {
                        poprawne--;
                    }
                    Przycisk_PlayPause.Visible = false;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            obiekt_[ID - 1].Picturebox.Image = obraz_obiektu[0, nr_grafik];
            timer1.Stop();
            stoper.Interval = 1;
            stoper.Start();
        }

        //wykonuje się, po tyknięciu stopera, ma na celu wybranie co robić, w zależności od części rundy
        private void stoper_tick(object sender, EventArgs e)
        {
            część_rundy++;
            switch (część_rundy)
            {
                case 2:
                    play = true;
                    Przycisk_PlayPause.Image = przycisk_playpause[0,0];
                    ToolTip_Przycisk_PlayPause.SetToolTip(this.Przycisk_PlayPause, "Pause");
                    for (int i = 0; i < ID; i++)
                    {
                        obiekt_[i].Picturebox.Image = null;
                    }
                    //Przycisk_Statystyki.Visible = true;
                    Przycisk_PlayPause.Visible = true;
                    Przycisk_Informacje.Visible = true;
                    stoper.Interval = 2000;
                    break;
                case 3:
                    stoper.Stop();
                    for (int z = 0; z < ID; z++)
                    {
                        obiekt_[z].Picturebox.Image = obraz_obiektu[0, nr_grafik];
                    }
                    //Przycisk_Statystyki.Visible = false;
                    Przycisk_Informacje.Visible = false;
                    Przycisk_PlayPause.Visible = false;
                    break;
                case 4:
                    //usuwa wszystkie pictureboxy z planszy
                    for (int i = ID; i >= 0; i--)
                    {
                        obiekt_[i] = null;
                        this.Controls.Remove(this.picturebox[i]);
                    }
                    nowa_runda();
                    break;
            }
        }

        //rozpoczyna nową rundę
        void nowa_runda()
        {
            zasłona.Visible = false;
            if (poprawne == 3)
            {
                runda++;
                poprawne = 0;
            }
            else if (poprawne == -1 && runda != 1)
            {
                runda--;
                poprawne = 2;
            }
            switch(poprawne)
            {
                case 0:
                    życia[0].Image = null;
                    życia[1].Image = null;
                    break;
                case 1:
                    życia[0].Image = pixel;
                    życia[1].Image = null;
                    break;
                case 2:
                    życia[0].Image = pixel;
                    życia[1].Image = pixel;
                    break;
            }
            if(kolejka==2)
            {
                kolejka = 0;
            }
            else
            {
                kolejka++;
            }    
            wybrano = false;
            if(runda<12)
            {
                ile_obiektow = runda + 3;
            }
            for (ID = 0; ID <= ile_obiektow; ID++)
            {
                picturebox[ID] = new PictureBox();
                this.Controls.Add(picturebox[ID]);
                Obiekt obiekt = new Obiekt(ID, picturebox[ID]);
                obiekt_[ID] = obiekt;
                this.picturebox[ID].MouseHover += new EventHandler(hover);
                this.picturebox[ID].MouseLeave += new EventHandler(leave);
                this.picturebox[ID].Click += new System.EventHandler(click);
                picturebox[ID].Name = ID.ToString();
                //nadaje odpowiednim pictureboxom odpowiednie reakcje na kliknięcie
                if(ID<ile_obiektow)
                {
                    obiekt_[ID].Picturebox.Image = obraz_obiektu[0, nr_grafik];
                }
                else
                {
                    obiekt_[ID].Picturebox.BackColor = Color.Transparent;
                }
            }
            lbl_rundy.Text = "Runda: " + runda;
            //update_statystyk();
            Przycisk_Informacje.Visible = true;
            Przycisk_PlayPause.Visible = false;
            część_rundy = 1;
            stoper.Interval = 5000;
            stoper.Start();
        }

        private void hover(object sender, EventArgs e)
        {
            if(część_rundy ==3&&wybrano==false)
            { 
                PictureBox picturebox = sender as PictureBox;
                picturebox.Image = obraz_obiektu[1, nr_grafik];
                Point pozycja = picturebox.Location;
                picturebox.Size = new System.Drawing.Size(100,100);
                picturebox.Location = new System.Drawing.Point(pozycja.X-10,pozycja.Y-10);
                mouseon = 1;
            }

        }

        private void leave(object sender, EventArgs e)
        {
            if(mouseon==1&&wybrano==false)
            {
                PictureBox picturebox = sender as PictureBox;
                picturebox.Image = obraz_obiektu[0, nr_grafik];
                Point pozycja = picturebox.Location;
                picturebox.Size = new System.Drawing.Size(80, 80);
                picturebox.Location = new System.Drawing.Point(pozycja.X + 10, pozycja.Y + 10);
                mouseon = 0;
            }

        }
    }

    public class Obiekt
    {
        public PictureBox Picturebox;
        public int Pozycja_x, Pozycja_y;
        public int ID;

        //konstruktor
        public Obiekt(int ID, PictureBox picturebox)
        {
            this.Losuj_pozycję();
            this.Picturebox=picturebox;
            this.Picturebox.BackColor = Color.Transparent;
            this.ID = ID;
            this.Picturebox.Size = new System.Drawing.Size(80, 80);
            this.Picturebox.Location = new System.Drawing.Point(this.Pozycja_x, this.Pozycja_y);
        }

        //losuje pozycję dla danego obiektu
        void Losuj_pozycję()
        {
            bool czy_losowac=false;
            Random a = new Random();
            int x=0, y=0;
            do
            {
                x = a.Next(0, 13) * 80+5;
                y = a.Next(0, 8) * 80+5;
                czy_losowac = false; 
                //sprawdza, czy wylosowana pozycja nie jest już zajęta
                for (int k = 0; k < 3;k++)
                {
                    for (int i = 0; i < Form1.ID; i++)
                    {
                        if (x == Form1.wykozystane_pola[k, i, 0] && y == Form1.wykozystane_pola[k, i, 1])
                        {
                            czy_losowac = true;
                        }
                    }
                }
            }while(czy_losowac==true);
            this.Pozycja_x = x;
            this.Pozycja_y = y;
            Form1.wykozystane_pola[Form1.kolejka, Form1.ID, 0] = x;
            Form1.wykozystane_pola[Form1.kolejka, Form1.ID, 1] = y;
        }
    }
}
