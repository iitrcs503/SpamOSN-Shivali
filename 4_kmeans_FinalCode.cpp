#include <iostream>
#include <sstream>
#include <fstream>
#include <string>
#include<stdio.h>
#include<stdlib.h>
#define rows (1000)
#include<conio.h>
#include<math.h>
#include<algorithm>

#define cols 6
using namespace std;
double calc_euclDist(long *k1, long * k2){
	double dist=0.0;
	for(int i=1; i<cols-1; i++){
		dist+=pow((k1[i]-k2[i]),2);
	}
	dist=sqrt(dist);
	return dist;
}
void calc_accuracy(long  k1[][cols], long k2[][cols], int i2, int i3){
	int count0_k1=0, count1_k1=0, count0_k2=0, count1_k2=0, k1_stat=-1, k2_stat=-1;
	double acc=0.0;
	for(int i=0; i<i2; i++){
		if(k1[i][cols-1]==0.0){
			count0_k1++;
		}
		else
			count1_k1++;
	}
	if(count0_k1>count1_k1){
		k1_stat=0;
	}
	else
		k1_stat=1;
	for(int i=0; i<i3; i++){
	
		if(k2[i][cols-1]==0){
			count0_k2++;
		}
		else
			count1_k2++;
	}
	if(count0_k2>count1_k2)
		k2_stat=0;
	else
		k2_stat=1;
	if(k1_stat==k2_stat)
		cout<<"0 accuracy";
	else{
		if(k1_stat==0){
			acc=((double)count0_k1+count1_k2)/rows;
		}
		else{
			acc=((double)count1_k1+count0_k2)/rows;
		}
	}
	cout<<"\nThe accuracy is: "<<acc*100.0<<"%";
}
int main()
{
    int i1,i2,i3;
	double t1,t2;
 
    long k0[rows][cols];
    long k1[rows][cols];
    long k2[rows][cols];
  	int row=0, col=0;
//    int k0[100][5];
//    cout<<"\nEnter "<<count<<" tuples:\n";
    std::ifstream fin("inputCSV_kmeans.csv");
    std::string line;
    while(std::getline(fin,line))
    {
        std::stringstream  lineStream(line);
        std::string        cell;
//        cols=0;
		col=0;
        while(std::getline(lineStream,cell,','))
        {
        	
//        	cout<<"Cell is: ";
//            cout<<cell<<"\n";
            string s=cell;
			int x=0;
 
			stringstream convert(s);//object from the class stringstream
			convert>>x; 
            
            k0[row][col]=x;
//            cout<<"\n";
			col++;
        }
        
        row++;
    }
    /*
    cout<<"k0 array is:\n";
	for(int x=0; x<rows; x++){
		for(int y=0; y<cols; y++){
			cout<<"\nk0["<<x<<"]["<<y<<"]="<<k0[x][y];
		}
	}
	*/
    /*
    char line[]=NULL;
    int rows=0, cols=0;
    char *arr=NULL;
    while(getline(fin, line)){
    	arr=strtok(line,",");
    	while(arr!=NULL){
    		k0[rows][cols]=arr;
    		arr = strtok (NULL, ",");
    		cols++;
		}
    	
    	line=NULL;
    	rows++;
	}
	*/
//    for(string line; getline(fin, line);){
//    	
//	}
     
 
 
    //initial means
    long * m1=k0[0];
    long * m2=k0[1];
    
 
//    cout<<"\n Enter initial mean 1:";
//    cin>>m1;
//    cout<<"\n Enter initial mean 2:";
//    cin>>m2;
     
    long * om1=NULL, * om2=NULL;    //old means
//    fill_n(om1,cols,0);
//    fill_n(om2,cols,0);
 	int flag1=0, flag2=0;
 	long m[cols];
 	long tempm[cols];
    do
    {
     
    //saving old means
    om1=m1;
    om2=m2;
    cout<<"Old means:\n1:\n";
    for(int i=0; i<cols; i++){
    	cout<<om1[i]<<" ";
	}
	cout<<"\n2:\n";
	for(int i=0; i<cols; i++){
    	cout<<om2[i]<<" ";
	}
// 	m1[cols]={0};
// 	m2[cols]={0};
    //creating clusters
    i1=i2=i3=0;
    
    for(i1=0;i1<rows;i1++)
    {
        //calculating distance to means
        t1=calc_euclDist(k0[i1],m1);
//        if(t1<0){t1=-t1;}
 
        t2=calc_euclDist(k0[i1],m2);
//        if(t2<0){t2=-t2;}
// 		cout<<"\nt1="<<t1<<"\nt2="<<t2;
        if(t1<t2)
        {
            //near to first mean
            for(int c=0; c<cols; c++){
            	k1[i2][c]=k0[i1][c];
			}
            i2++;
//            cout<<"\nPushing in k1";
        }
        else
        {
            //near to second mean
            for(int c=0; c<cols; c++){
            	k2[i3][c]=k0[i1][c];
        	}
            i3++;
//            cout<<"\nPushing in k2";
        }
 
    }
 
    t2=0;
//    m[cols]={0};
	fill_n(m,cols,0);
    cout<<"\nBefore calc new mean: m is for cluster1: ";
    for(int i=0; i<cols; i++){
		cout<<m[i]<<" ";
	}
    //calculating new mean in k1
    for(int j=1; j<cols-1; j++){
    	for(int i=0;i<i2;i++)
	    {	    	
//	        t2=t2+k1[t1];
			m[j]+=k1[i][j];
	    }
	    cout<<"\nSum m["<<j<<"]="<<m[j];
	    m[j]/=rows;
	}
	m[0]=1111;
	m[cols-1]=-1;
	m1=m;
	cout<<"\nmean/centroid for cluster 1:";
	for(int i=0; i<cols; i++){
		cout<<m1[i]<<" ";
	}
//    m1=t2/i2;
 	
 	
    t2=0;
    
    fill_n(tempm,cols,0);
    cout<<"\nBefore calc new mean: m is for cluster2: ";
    for(int i=0; i<cols; i++){
		cout<<tempm[i]<<" ";
	}
    for(int j=1; j<cols-1; j++){
    	for(int i=0;i<i3;i++)
	    {	    	
//	        t2=t2+k1[t1];
			tempm[j]+=k2[i][j];
	    }
	    cout<<"\nSum m["<<j<<"]="<<m[j];
	    tempm[j]/=rows;
	}
	tempm[0]=2222;
	tempm[cols-1]=-1;
	m2=tempm;
	cout<<"\nmean/centroid for cluster 2:";
	for(int i=0; i<cols; i++){
		cout<<m2[i]<<" ";
	}
    
 
    //printing clusters
    cout<<"\nCluster 1:\n";
    for(int i=0;i<i2;i++)
    {
    	cout<<"Profile No: "<<k1[i][0];
//    	for(int j=0; j<cols; j++){
//    		cout<<k1[i][j]<<" ";
//		}
		cout<<"\n";
        
    }
//    cout<<"\nm1="<<m1;
 
    cout<<"\nCluster 2:\n";
    for(int i=0;i<i3;i++)
    {
    	cout<<"Profile No: "<<k2[i][0];
//    	for(int j=0; j<cols; j++){
//    		cout<<k1[i][j]<<" ";
//		}
		cout<<"\n";
        
    }
//    cout<<"\nm2="<<m2;
 
    cout<<"\n ----";
    flag1=0; flag2=0;
    for(int i=1; i<cols-1; i++){
    	if(m1[i]!=om1[i]){
    		flag1=1;
//    		break;
		}
    		
    	if(m2[i]!=om2[i]){
    		flag2=1;
//    		break;
		}
    		
	}
    }while(flag1 && flag2);
	//while(m1!=om1&&m2!=om2);//!(m1==om1 or m2==om2)
    
 
    cout<<"\n Clusters created";
 
    //ending
    calc_accuracy(k1,k2,i2,i3);
    getch();
    return 0;
}
