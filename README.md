[![License: MIT](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/carreraSilvio/BrightPoolSystem/blob/master/LICENSE.md)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/fda86a824b52472f83bd189758663a15)](https://app.codacy.com/gh/carreraSilvio/BrightPoolSystem/dashboard?utm_source=gh&utm_medium=referral&utm_content=&utm_campaign=Badge_grade)

# Bright Pool System
A pooling system that creates and manages access to your pool of objects. 

![Imgur](https://i.imgur.com/XcPNTTL.gif)

## Features
* Create pools when it's best for your project.
* Avoid garbage collection by resuing objects.
* Use Aquire and Release methods as your "Instantiate" and "Destroy" objects.
* Use events to notify other systems when objects are aquired and released.

## Prerequisites
Unity 2018.3 and up

## Install

### Unity 2019.3
1. Open the package manager and point to the rep url

![Imgur](https://i.imgur.com/iYGgINz.png)

### Before Unity 2019.3

#### Option A
1. Open the manifest
2. Add the repo url either via https or ssh

		{
    		"dependencies": {
        		"com.brightlib.poolsystem": "https://github.com/carreraSilvio/poolsystem.git"
    		}
		}

#### Option B
1. Clone or download the project zip
2. Copy the repo inside your project assets folder

## Usage

### Create Pools
#### Option A
1. Add PoolSystemInitializer behaviour
2. Edit the PoolConfig array by adding a string id and a poolable prefab
3. Run the game and you'll see the pool in your hierachy

#### Option B
```csharp
public string poolableId;
public GameObject poolablePrefab;
public int poolSize;

void Awake()
{
    PoolSystem.CreatePool(poolableId, poolablePrefab, poolSize);
}
```

### Fetch Available
```csharp
private void Shoot()
{
    if (PoolSystem.TryFetchAvailable("Bullet", out PrefabPoolable bullet))
    {
        bullet.transform.position = transform.position;
    }
    else
    {
        Debug.Log("no more bullets available in pool");
    }
}
```

### Listen to Events
```csharp
void OnEnable()
{
    PoolSystem.AddListener("Bullet", PoolEvent.OnAquire, HandleBulletAquire);
}

private void HandleBulletAquire(string poolableName, int poolSize, int poolableInUse)
{
	Debug.Log("A bullet was aquired.");
}
```
