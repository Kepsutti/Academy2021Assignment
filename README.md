# Academy2021Assignment

Gameplay features

At its current state the game seems to have little incentive to make players feel any sense of hurry, which might have the effect of stalling the gamplay and making it feel slow if there's nothing worth taking risks for instead of stopping to wait for the next color opening.

For this I would suggest timed collectables, for example a gold star worth more points, which as soon as it becomes visible on the screen, starts to fade away and vanishes if the player doesn't collect it in time. A more mild version of this, let's say a silver star, could be an otherwise regular star, except it also moves sideways, so that players might need to stop for a second in order to collect it. It should still move fast enough to not make it another stalling element for the gameplay, just out of reach enough to require a moment of action since the current white stars can just be raced through without a second thought.

There could also be obstacles that are slightly less predictable. For example, an arrow type block could appear from the side and move through the screen on the x axis. This would need to be implemented only in situations where there is enough room to drop lower on the screen to avoid it, in case there happens to be an obstacle right above it which wouldn't offer an opening before being hit by the block. The block would also have to move slowly enough to give players ample time to react to it, as it would be far too frustrating if one just shot from the side to suddenly end the game.

I would also personally feel more accomplised if the height that I've climbed all the way to was visible. It could be calculated from the camera's position so it wouldn't be constantly changing. I would also add some dotted lines at certain heights, like at each new 100 or 1000 depending on the scale of measure, that also show the number of the line's height. To me such markers would feel more exciting.

I also got this stupid idea that you could play locally with a friend on the same device, with the same ball, and each player can only tap their side of the screen, and the dot jumps in turns - so if one player tries to jump twice in a row only the first one would register until the other player has jumped first. But this idea mostly just feels like a great way to ruin friendships. I have no idea if it would be fun, but at least it would be a relatively easy mechanic to add for testing.



Prototype implementation

I decided to work with fewer scripts that handle more functions at the same time, though I get the feeling it might be wiser to make more clear divisions in terms of responsibilities per script by dividing them more. The scripts are relatively short but already started to feel like I need to search for the function I'm looking to edit.

A game manager script is attached to an empty object, which takes care of initializing the level and keeps an updating list of both collectibles and obstacles. Once an object is no longer visible below the player it is destroyed and a random new one appears above the current objects.

The player script handles collision with all objects based on tags - it sends information of collected points to the game manager and triggers particle and sound effects. The camera has a child at the bottom of its field of view, which shares its GameOver tag functionality with the obstacles.

Colliding with obstacles is handled through layers, where an object of a certain color is also placed on a layer named after said color. The color layers can not collide with others sharing its own layer, as defined in the collision matrix. When the player changes color, so changes its layer.

The last thing I made was the wall type obstacle, which is clearly the least polished part as it stands. While working on it I realized that it probably would've been wiser to include the array of available colors in the game manager instead of the player script. That would've saved me from the wall's awkward switch case.

If there's a chance, I would like to hear feedback about my solutions and what I would be better off doing some another way.
