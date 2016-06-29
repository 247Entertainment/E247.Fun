###E247.Fun
#Maybe&lt;T&gt;

```Maybe<T>``` is used to represent a value that might not be available. While it is an option to use ```null``` for reference types, it isn't explicit in the type and returning ```null``` can lead to problems when it's not handled properly, using ```Maybe<T>``` instead makes it obvious that the potential lack of a value needs to be handled, and provides some handy methods for dealing with boilerplate based around that.

##Making a Maybe
To convert a typed value to a ```Maybe``` you can simply call the ```.ToMaybe()``` extension method, this will even handle if that value is ```null``` by creating an empty ```Maybe``` of that type.

##Checking for a Value
As a ```Maybe``` has two potential states that should be handled it is possible to match on them using the ```.Match``` method which takes 2 functions, one for handling each state:

    var whatWasIt = 
        someMaybeValue.Match(
            Some: theValue => $"It was {theValue}",
            None: () => "It was empty");

In the example above, the variable ```whatWasIt``` is set to a different string value depending on if the ```Maybe``` contained a value or not.

The function provided for ```Some``` should take a parameter of the same type as the ```Maybe``` value, the function provided for ```None``` should take no parameters and return the same type as the ```Some``` function. The ```.Match()``` method will then return the value returned by whichever function was invoked. By convention the parameter labels should be included to make the code more readable.

##Clever Extensions
The ```.Match()``` method handles the boilerplate for the situation where _if this has a value do one thing, otherwise do something else_. There are other situations that frequently come up when working with a ```Maybe``` that also have handy methods to save on boilerplate, ```.Map()``` and ```.Bind()``` can help to handle these.

```.Map()``` handles the situation where you only want to call another method if the ```Maybe``` contains a value, for example, if you want to transform the data. The function passed into a ```.Map()``` should take the type contained in the ```Maybe``` as a parameter, and return a different non-```Maybe``` type. If the ```Maybe``` contains a value ```.Map()``` will call the function with the value inside, and return a new ```Maybe``` of the return type of the function, if it's empty ```.Map()``` will return an empty ```Maybe``` of the return type of the function. The example below takes a ```Maybe<Caterpillar>``` and returns a ```Maybe<Butterfly>```:

    private static Butterfly Metamorphosis(Caterpillar c)
    {
        //nature is wonderful
    }

    ...

    //this returns a Maybe<Butterfly>
    maybeCaterpillar.Map(Metamorphosis);

```.Bind()``` is similar to ```.Map()``` but works with a function that also returns a ```Maybe<T>``` and so avoids wrapping the value twice into a ```Maybe<Maybe<T>>```:

    private static Maybe<Butterfly> Metamorphosis(Caterpillar c)
    {
        //nature is wonderful
        //but sometimes things go wrong...
    }

    ...

    //this would return Maybe<Maybe<Butterfly>> which is silly
    maybeCaterpillar.Map(Metamorphosis)

    //this just returns Maybe<Butterfly>
    maybeCaterpillar.Bind(Metamorphosis)

##Async
For working with async methods, there are also async versions of these functions: ```.MatchAsync()```, ```.MapAsync()```, and ```.BindAsync()```.

##Plain Old If
If you need to check the state of a ```Maybe``` in a way that doesn't quite fit into one of the methods above, typically for something with side effects it's possible to check on the value using a plain old if statement and either the ```.HasValue``` property or ```.Any()``` method, and then extract the value with the ```.Value``` property. But be warned, trying to access the value of an empty ```Maybe``` will throw!