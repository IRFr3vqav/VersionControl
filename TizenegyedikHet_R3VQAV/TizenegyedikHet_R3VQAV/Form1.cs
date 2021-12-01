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

        public Form1()
        {
            InitializeComponent();

            Population = GetPopulation(@"C:\Temp\nép-teszt.csv");
            BirthProbabilities = GetBirthProbabilities(@"C:\Temp\születés.csv");
            DeathProbabilities = GetDeathProbabilities(@"C:\Temp\halál.csv");


            for (int year = 2005; year <= 2024; year++)
            {
                for (int i = 0; i < Population.Count; i++)
                {

                }

                int nbrOfMales = (from x in Population
                                  where x.Gender == Gender.Male && x.IsAlive
                                  select x).Count();
                int nbrOfFemales = (from x in Population
                                    where x.Gender == Gender.Female && x.IsAlive
                                    select x).Count();
                Console.WriteLine(string.Format("Év:{0} Férfiak száma:{1} Nők száma:{2}", year, nbrOfMales, nbrOfFemales));
            }
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

    }
}
