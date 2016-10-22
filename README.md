# SpamOSN-Shivali
Spam Detection in online Social Networks- By: Raghu Tak (16535031), Renuka Sharma (16535035), Shivali Sharma (16535037)

We are using Facebook as the online social network and have got the data for spam and normal users.
We have implemented 3 approaches for spam detection and are calculating the accuracy for each of the approaches. The approaches are:
1. Naive Bayesian Classification
2. kMeans clustering
3. Markov clustering

The first approach, Naive Bayesian Classification is a supervised one. We are training the classifier with the help of training dataset and then are classifying the elements in the test data set.
kMeans and Markov are unsupervised approaches.
In kMeans, we are iteratively clustering the dataset into two clusters: Spam and non- spam one, by finding centroids in each cluster and putting the test set into the nearest one cluster.
Markov clustering is based on simulation of flow in graphs.
