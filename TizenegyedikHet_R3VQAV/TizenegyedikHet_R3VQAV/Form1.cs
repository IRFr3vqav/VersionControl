using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TizenegyedikHet_R3VQAV.Entities;

namespace TizenegyedikHet_R3VQAV
{
    public partial class Form1 : Form
    {
        List<Person> Population = new List<Person>();
        List<BirthProbability> BirthProbabilities = new List<BirthProbability>();
        List<DeathProbability> DeathProbabilities = new List<DeathProbability>();

        Random rng = new Random(1234);

        List<Person> Ferfiak = new List<Person>();
        List<Person> Nok = new List<Person>();
        public Form1()
        {
            InitializeComponent();

            BirthProbabilities = GetBirthProbabilities(@"C:\Temp\születés.csv");
            DeathProbabilities = GetDeathProbabilities(@"C:\Temp\halál.csv");

        }

        private void Simulation()
        {
            richTextBox1.Clear();
            Ferfiak.Clear();
            Nok.Clear();

            for (int year = 2005; year <= int.Parse(numericUpDown1.Text); year++)
            {
                for (int i = 0; i < Population.Count; i++)
                {
                    SimStep(year, Population[i]);

                    if (Population[i].Gender==Gender.Male && Population[i].IsAlive==true)
                    {
                        Ferfiak.Add(new Person()
                        {
                            BirthYear=Population[i].BirthYear,
                            Gender=Population[i].Gender,
                            NbrOfChildren=Population[i].NbrOfChildren,
                            IsAlive=Population[i].IsAlive
                        });                            
                    }
                    else if (Population[i].Gender == Gender.Female && Population[i].IsAlive == true)
                    {
                        Nok.Add(new Person()
                        {
                            BirthYear = Population[i].BirthYear,
                            Gender = Population[i].Gender,
                            NbrOfChildren = Population[i].NbrOfChildren,
                            IsAlive = Population[i].IsAlive
                        });
                    }
                }

                int nbrOfMales = (from x in Population
                                  where x.Gender == Gender.Male && x.IsAlive
                                  select x).Count();
                int nbrOfFemales = (from x in Population
                                    where x.Gender == Gender.Female && x.IsAlive
                                    select x).Count();
                Console.WriteLine(string.Format("Év:{0} Férfiak száma:{1} Nők száma:{2}", year, nbrOfMales, nbrOfFemales));
            }

            DisplayResults();
        }

        public List<Person> GetPopulation(string csvpath)
        {
            List<Person> population = new List<Person>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var sor = sr.ReadLine().Split(';');
                    population.Add(new Person()
                    {
                        BirthYear = int.Parse(sor[0]),
                        Gender = (Gender)Enum.Parse(typeof(Gender), sor[1]),
                        NbrOfChildren = int.Parse(sor[2])
                    });
                }
            }

            return population;
        }

        public List<BirthProbability> GetBirthProbabilities(string csvpath)
        {
            List<BirthProbability> birthprobabilities = new List<BirthProbability>();

            using(StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var sor = sr.ReadLine().Split(';');
                    birthprobabilities.Add(new BirthProbability()
                    {
                        Age = int.Parse(sor[0]),
                        NbrOfChildren=int.Parse(sor[1]),
                        BirthProb=double.Parse(sor[2])
                    }) ; 
                }
            }

            return birthprobabilities;
        }

        public List<DeathProbability> GetDeathProbabilities(string csvpath)
        {
            List<DeathProbability> deathprobabilities = new List<DeathProbability>();

            using(StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var sor = sr.ReadLine().Split(';');
                    deathprobabilities.Add(new DeathProbability()
                    {
                        Gender = (Gender)Enum.Parse(typeof(Gender), sor[0]),
                        Age = int.Parse(sor[1]),
                        DeathProb = double.Parse(sor[2])
                    });
                }
            }

            return deathprobabilities;
        }

        private void SimStep(int year, Person p)
        {
            if (!p.IsAlive) return; //csak akkor megyunk tovabb, ha meg el a vizsgalt egyen

            byte age = (byte)(year - p.BirthYear); //eletkor kiszamolasa

            //Halalozasi valoszinuseg kikeresese, halal kezelese
            double pDeath = (from x in DeathProbabilities
                             where x.Gender == p.Gender && x.Age == age
                             select x.DeathProb).FirstOrDefault();

            // Meghal a szemely?
            if (rng.NextDouble() <= pDeath)
                p.IsAlive = false;

            //Szulesek kezelese, csak az elo noket nezzuk
            if (p.IsAlive==true && p.Gender==Gender.Female)
            {
                //szulesi valoszinuseg kikeresese
                double pBirth = (from x in BirthProbabilities
                                 where x.Age == age
                                 select x.BirthProb).FirstOrDefault();

                //Szuletik gyermek? Ha igen, akkor ujszulott hozzaadasa a population listahoz
                if (rng.NextDouble() <= pBirth)
                {
                    Person ujszulott = new Person();
                    ujszulott.BirthYear = year;
                    ujszulott.NbrOfChildren = 0;
                    ujszulott.Gender = (Gender)(rng.Next(1, 3));
                    Population.Add(ujszulott);
                }
            }

        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            Simulation();
        }

        private void btn_browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() != DialogResult.OK) return;
            txt_eleresi_utvonal.Text = ofd.FileName;

            Population = GetPopulation(txt_eleresi_utvonal.Text);
            
        }

        private void DisplayResults()
        {
            for (int year = 2005; year < int.Parse(numericUpDown1.Text); year++)
            {
                richTextBox1.Text += string.Format("Szimulációs év: {0}\n\tFiúk: {1}\n\tLányok: {2}\n\n", year, Ferfiak.Count, Nok.Count);
            }
        }
    }
}
