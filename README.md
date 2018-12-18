# GKKM-OKE18ChallengeTask2
Our student attempt to OKE2018 Challenge Task2 using CRF algorithm

## CRFModule
Module was tested on **Ubuntu 18.04** with **python 3.6.6**

In order to run CRF module, packages listed in *requirements.txt* file have to be installed. It can be done using *pip* package manager for python.  
To install using *pip*:
```bash
#!/usr/bin/env bash
pip3 install -r CRFModule/requirements.txt
```

Module consists of three submodules: *rdfparser*, *crftools* and *crfserving_app*. These submodules should be started in given order:  
1. *rdfparser* and *crftools* both using *setup.py* script (can be run only once, necessary to generate samples and deploy trained model)
2. crfserving_app using crfserving_app.py script (flask server, which serve predictions)

To run CRFModule using bash:
```bash
#!/usr/bin/env bash
cd CRFModule
python3 setup.py
# wait until process ends
python3 crfserving_app.py
```

To run CRFModule on windows:
```cmd
cd CRFModule
windows-run.bat /full/path/to/folder/CRFModule setup.py
# wait until process ends
windows-run.bat /full/path/to/folder/CRFModule crfserving_app
```