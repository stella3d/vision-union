vision-union
============================================================

Vision Union is a _collection of computer vision workers that becomes more powerful together._

The 'workers' are image processing and 2D convolutional neural network building blocks, implemented as High-Performance C# jobs.

This project is a prototype and proof of concept right now.


# Why ?

Efficient runtime inference / serving of existing computer vision models on the cpu, by leveraging Burst and the job system.


## Builbing Blocks

### Input Processing

Utilities such as RGB -> grayscale conversion and integral image calculation.

### Convolution

A system for convolving generic 2D kernels over images

### Pooling

Max & average pooling implementations

### Activation

ReLu activation, TODO - softmax 
