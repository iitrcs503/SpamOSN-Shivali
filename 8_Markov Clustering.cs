using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Markov_Clustering
{
    class Program
    {
        static void Main(string[] args)
        {
            Random r = new Random();
            double TotalProfiles=995;
            double SpamProfiles=0;
            double NormalProfiles=0;
            var reader = new StreamReader(File.OpenRead(@"Markov_input.csv"));
            
            List<String> Spams = new List<string>();
            List<String> Normals = new List<string>();
            
            string line = reader.ReadLine();
            while (line != null)
            {
                var values = line.Split(',');

                if (Int32.Parse(values[4]) == 0)
                {
                    Spams.Add(values[0]);
                    SpamProfiles++;
                }
                else
                {
                    Normals.Add(values[0]);
                    NormalProfiles++;
                }
                line = reader.ReadLine();
            }
            
            double[] initial_Dist=new double[2]{SpamProfiles/TotalProfiles,NormalProfiles/TotalProfiles};
            reader.DiscardBufferedData();
            reader.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
            
            double PredictedTotalSpam = 0;
            double PredictedTotalNormal = 0;

            string line1 = reader.ReadLine();
            
            while (line1!=null)
            {
                double one = 1.0;
                var values = line1.Split(',');
                double SpamFriends = Convert.ToDouble(values[7]);
                //double NormalFriends = double.Parse(values[8]);
                double TotFriends=Convert.ToDouble(values[2]);
                double SpamProb = SpamFriends / TotFriends;
                
                double NormalProb = one - SpamProb;

                double Not_a_Profile_Prob = TotFriends / (TotalProfiles - 1);
                double complement_NAPP = 1 - Not_a_Profile_Prob;

               // double SpamFinalProbality = initial_Dist[0] * SpamProb + initial_Dist[1] * Not_a_Profile_Prob;
                //double NormalFinalProbability = initial_Dist[0] * NormalProb + initial_Dist[1] * complement_NAPP;

                double NormalFinalProbability = 1 / (1 + (Not_a_Profile_Prob / NormalProb));
                double SpamFinalProbality = 1 - NormalFinalProbability;


                
                if (SpamFinalProbality > NormalFinalProbability || (SpamProb - NormalProb) > 0.05)
                {
                    PredictedTotalSpam++;
                }
                else
                {
                    
                    if (SpamProb < NormalProb)
                    {
                        PredictedTotalNormal++;
                    }
                    
                }
                
                line1 = reader.ReadLine();
            }
            double Recall_Spam_Markov = PredictedTotalSpam / SpamProfiles;
            double Recall_Normal_Markov = PredictedTotalNormal / NormalProfiles;
            Console.WriteLine("Recall for Spam and Normal profle detection");
            Console.WriteLine(Recall_Spam_Markov);
            Console.WriteLine(Recall_Normal_Markov);

            Console.WriteLine("\nInitially Spam and Normal Profiles");
            Console.WriteLine(SpamProfiles);
            Console.WriteLine(NormalProfiles);

            Console.WriteLine("\nRequired Precision which can be found by Spam Profiles/Total Profiles for");
            Console.WriteLine("spam profiles and Normal Profiles/Total Profiles for Normal Profiles");

            double Precision_Spam_Required = SpamProfiles / TotalProfiles;
            double Precision_Normal_Required = NormalProfiles / TotalProfiles;
            Console.WriteLine(Precision_Spam_Required);
            Console.WriteLine(Precision_Normal_Required);

            Console.WriteLine("\nPrecision calculated for this approach");
            double Precision_Spam_Markov = PredictedTotalSpam / TotalProfiles;
            double Precision_Normal_Markov = PredictedTotalNormal / TotalProfiles;
            Console.WriteLine(Precision_Spam_Markov);
            Console.WriteLine(Precision_Normal_Required);

            Console.WriteLine("\nAccuracy for spam and normal profile are respectively as follows");
            double a = (1.0 - ((Precision_Spam_Required- Precision_Spam_Markov)/Precision_Spam_Required));
            double Accuracy_Spam = a * 100.0;
            double b = (1.0 - ((Precision_Normal_Required- Precision_Normal_Markov) / Precision_Normal_Required));
            double Accuracy_Normal = b * 100.0;
            Console.WriteLine(Accuracy_Spam);
            Console.WriteLine(Accuracy_Normal);
           

            Console.WriteLine("\nF Measure calculated for this approach");
            double fact1 = 0.5 / Precision_Spam_Markov;
            double fact2 = 0.5 / Recall_Spam_Markov;
            double FMeasure_Spam = 1.0 / (fact1 + fact2);

            double fact3 = 0.5 / Precision_Normal_Markov;
            double fact4 = 0.5 / Recall_Normal_Markov;
            double FMeasure_Normal = 1.0 / (fact3 + fact4);

            Console.WriteLine("For spam");
            Console.WriteLine(FMeasure_Spam);
            Console.WriteLine("For Normal");
            Console.WriteLine(FMeasure_Normal);
            Console.ReadLine();
#region      
            //Code to create the required CSV
           /* Random isSpam_Random=new Random();            
            Random URl_Shared_Random = new Random();
            Random Total_Friends_Random = new Random();
            Random Likes_Random = new Random();
            Random Friend_Number=new Random();


            var writer = new StreamWriter(File.OpenWrite(@"E:\test2.csv"));            
            int LikesOnPost = 0;
            int Friends = 0;
            int URL_Shared = 0;
            int Total_Spam_Profiles = 0;
            int Total_Normal_Profiles = 0;

            for (int i = 1; i <= 1000; i++)
            {
                int Category = isSpam_Random.Next(0, 2);
                if (Category==0)
                {
                    LikesOnPost = Likes_Random.Next(0, 5);
                    Friends = Total_Friends_Random.Next(1, 8);
                    URL_Shared = URl_Shared_Random.Next(10, 51);
                    string lineToWrite = i.ToString()+","+ LikesOnPost.ToString()+","+Friends.ToString()+","+URL_Shared.ToString()+","+Category.ToString();
                    writer.WriteLine(lineToWrite);
                }
                else
                {
                    LikesOnPost = Likes_Random.Next(5, 100);
                    Friends = Total_Friends_Random.Next(8, 20);
                    URL_Shared = URl_Shared_Random.Next(0, 7);
                    string lineToWrite = i.ToString() + "," + LikesOnPost.ToString() + "," + Friends.ToString() + "," + URL_Shared.ToString() + "," + Category.ToString();
                    writer.WriteLine(lineToWrite);
                }
            }
            
            writer.Close();

            var reader1 = new StreamReader(File.OpenRead(@"E:\test2.csv"));
            var writer1 = new StreamWriter(File.OpenWrite(@"E:\test3.csv"));

            List<String> Spams = new List<string>();
            List<String> Normals = new List<string>();

            string line = reader1.ReadLine();
            while (line != null)
            {
                var values = line.Split(',');

                if (Int32.Parse(values[4]) == 0)
                {
                    Spams.Add(values[0]);
                    Total_Spam_Profiles++;
                }
                else
                {
                    Normals.Add(values[0]);
                    Total_Normal_Profiles++;
                }
                line = reader1.ReadLine();
            }
            reader1.DiscardBufferedData();
            reader1.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);


            line = reader1.ReadLine();
            while (line != null)
            {
                int SpamFrnds = 0;
                int NormalFrnds = 0;
                string sb = "";
                string SpamFrndStr = "";
                string NormalFrndStr = "";
                var values = line.Split(',');
                int num_fr = Int32.Parse(values[2]);

                for (int j = 0; j < num_fr; j++)
                {

                    int Frnd = Friend_Number.Next(1, 1001);
                    if (Spams.Contains(Frnd.ToString()))
                    {
                        SpamFrndStr = SpamFrndStr + Frnd.ToString() + " ";
                        SpamFrnds++;
                    }
                    else
                    {
                        NormalFrndStr = NormalFrndStr + Frnd.ToString() + " ";
                        NormalFrnds++;
                    }
                }
                sb = line + "," + SpamFrndStr + "," + NormalFrndStr + "," + SpamFrnds.ToString() + "," + NormalFrnds.ToString();


                writer1.WriteLine(sb);
                line = reader1.ReadLine();
            */

#endregion
//Comment Block

            
            
        }
    }
}
