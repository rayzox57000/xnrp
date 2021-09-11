using Sandbox.Jobs.Category;
using Sandbox.Jobs.Activity;
using System.Collections.Generic;

namespace Sandbox.Jobs.Categories
{
    public class JobsCategories : NetworkComponent
    {
      public List<JobsCategory> CategList {get;} = new();
      public Job Citizen {get;} = new(-1,"Citoyen","","",0,0.0f,100.0f,false);


       public void BulkAdd(params JobsCategory[] categories)
       {
         foreach (JobsCategory category in categories) { this.CategList.Add(category); }
       }

       public JobsCategories()
      {
        JobsCategory Legal = new("Legal","Un Boulot Stable","legal.png");
        JobsCategory ForceOrder = new("Forces de l'ordre","Servir et Protéger","security.png");
        JobsCategory Illegal = new("Illegal","Un Boulot Instable.","wanted.png");

        JobsActivity Faux = new(1,"Métier de Couverture (Espion)","Transformez-vous en Espion. Infiltrez les gangs avec votre métier de couverture. Vous travaillez automatiquement avec la police.","fake.png");
        JobsActivity Voleur = new(1,"Voleur","À bord de votre véhicule, volez vos propres clients, profitez d'un moment d'intention pour dérober tout ce qui est possible de prendre. Vous faites automatiquement partie des bandits.","robber.png");
        JobsActivity Psycopathe = new(2,"Psycopathe","","");
        JobsActivity Gangster = new(3,"Gangster","","");
        JobsActivity Arnaqueur = new(4,"Arnaqueur","C'est la crise, vous vous fichez de la concurrence votre but va être d'embrouiller votre client pour qu'il vous donne un maximum, mais attention ! Vous pouvez tomber sur la mauvaise personne à arnaquer alors soyez prudent. Vous faites automatiquement partie des bandits.","scammer.png");
        JobsActivity Ubeurre = new(5,"Ubeurre","Vous souhaitez travailler pour la célèbre entreprise de déplacement utilitaire qu'est Ubeurre ? Alors soyez le bienvenu ! Attention, c'est quelque chose de difficile, les clients sont exigent et n'hésiterons pas à vous mettre une mauvaise note si vous vous comportez mal ... Le salaire est réduit, mais les primes et les pourboires sont intéressants ;).","ubeurre.png");


        Job Cop = new(1,"Policier","test1","cop.png",5,12.0f);
        Job Cop1 = new(2,"Chef de la Police","test2","cop.png",1,12.5f,120.0f);
        Job Cop2 = new(3,"Bac","test3","bac.png",3,12.2f);
        Job Cop3 = new(4,"Chef de la Bac","test4","bac_chief.png",1,13.2f);
        Job Cop4 = new(5,"Chien Policier","test5","dog.png",2,9.2f);

        Job Legal0 = new(6,"Sans Travail","test6","worker.png",0,5.2f);
        Job Legal1= new(7,"Chauffeur TAXI","test7","taxi.png",3,11.2f);
        Job Legal2 = new(8,"Cuisinier","test8","cuisinier.png",5,11.3f);
        Job Legal3 = new(9,"Chef Cuisinier","test9","chef_cuisinier.png",2,12.0f);
        Job Legal4 = new(10,"Boulanger","test10","bakering.png",5,11.2f);

        Legal0.AddBulkActivities(Voleur,Psycopathe,Gangster);
        Legal1.AddBulkActivities(Faux,Voleur,Arnaqueur,Ubeurre);

        Legal.AddBulkJobs(Citizen,Legal0,Legal1,Legal2,Legal3,Legal4);
        ForceOrder.AddBulkJobs(Cop,Cop1,Cop2,Cop3,Cop4);

        BulkAdd(Legal,ForceOrder,Illegal);
      }

    }
}