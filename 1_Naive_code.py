# Example of Naive Bayes implemented from Scratch in Python
import csv
import random
import math
 
def loadCsv(filename):
	lines = csv.reader(open(filename, "r"))
	dataset = list(lines)
	for i in range(len(dataset)):
		dataset[i] = [float(x) for x in dataset[i]]
	return dataset
 
def splitDataset(dataset, splitRatio):
	trainSize = int(len(dataset) * splitRatio)
	trainSet = []
	copy = list(dataset)
	while len(trainSet) < trainSize:
		index = random.randrange(len(copy))
		trainSet.append(copy.pop(index))
	return [trainSet, copy]
 
def separateByClass(dataset):
	separated = {}
	for i in range(len(dataset)):
		vector = dataset[i]
		if (vector[-1] not in separated):
			separated[vector[-1]] = []
		separated[vector[-1]].append(vector)
	return separated
 
def mean(numbers):
	return sum(numbers)/float(len(numbers))
 
def stdev(numbers):
	avg = mean(numbers)
	if(float(len(numbers)-1)!=0.0):
		variance = sum([pow(x-avg,2) for x in numbers])/float(len(numbers)-1)
		return math.sqrt(variance)
 
def summarize(dataset):
	summaries = [(mean(attribute), stdev(attribute)) for attribute in zip(*dataset)]
	del summaries[-1]
	#del summaries[0]- were del for profile id
	return summaries
 
def summarizeByClass(dataset):
	separated = separateByClass(dataset)
	summaries = {}
	for classValue, instances in separated.items():
		summaries[classValue] = summarize(instances)
	return summaries
 
def calculateProbability(x, mean, stdev):
	if((2*math.pow(float(stdev),2.0))!=0.0):
		exponent = math.exp(-(math.pow(x-float(mean),2)/(2*math.pow(stdev,2))))
		return (1 / (math.sqrt(2*math.pi) * stdev)) * exponent
 
def calculateClassProbabilities(summaries, inputVector,ps,pn):
	probabilities = {}
	#print("Entered into calcClassProb function...\n")
	for classValue, classSummaries in summaries.items():
		if(classValue==0):
			probabilities[classValue] = ps
		else:
			probabilities[classValue] = pn
		#print("prob[0]=")
		#print(probabilities[0])
		for i in range(len(classSummaries)):
			mean, stdev = classSummaries[i]
			x = inputVector[i]
			probabilities[classValue] *= calculateProbability(x, mean, stdev)
		#print("Prob[0]={0}, prob[1]={1}".format(probabilities[0],probabilities[1]))
		#print("prob[0]=")
		#print(probabilities[0])
		
	return probabilities
			
def predict(summaries, inputVector, ps, pn):
	probabilities = calculateClassProbabilities(summaries, inputVector, ps,pn)
	bestLabel, bestProb = None, -1
	for classValue, probability in probabilities.items():
		if bestLabel is None or probability > bestProb:
			bestProb = probability
			bestLabel = classValue
	return bestLabel
 
def getPredictions(summaries, testSet,ps,pn):
	predictions = []
	for i in range(len(testSet)):
		result = predict(summaries, testSet[i], ps, pn)
		predictions.append(result)
	return predictions
 
def getAccuracy(testSet, predictions):
	correct = 0
	for i in range(len(testSet)):
		if testSet[i][-1] == predictions[i]:
			correct += 1
	return (correct/float(len(testSet))) * 100.0
	
def calcProbForSpamNonSpam(trainingSet):
	spamCount=0
	normalCount=0
	count=0
	probSpam=1
	probNormal=1
	for i in trainingSet:
		if(i[-1]==0):
			spamCount+=1
		else:
			normalCount+=1
	count= spamCount+normalCount
	probSpam=spamCount/count
	probNormal=normalCount/count
	print("\nSpam prob= {0}\nNormal prob={1}".format(probSpam,probNormal))
	return probSpam,probNormal
 
def main():
	filename = 'inputCSV_naive.csv'
	splitRatio = 0.73
	dataset = loadCsv(filename)
	trainingSet, testSet = splitDataset(dataset, splitRatio)
	print("\nTrainingSet:\n{0}".format(trainingSet))
	ps,pn=calcProbForSpamNonSpam(trainingSet)
	print('Split {0} rows into train={1} and test={2} rows'.format(len(dataset), len(trainingSet), len(testSet)))
	# prepare model
	summaries = summarizeByClass(trainingSet)
	# test model
	predictions = getPredictions(summaries, testSet,ps,pn)
	accuracy = getAccuracy(testSet, predictions)
	print('Accuracy: {0}%'.format(accuracy))
 
main()