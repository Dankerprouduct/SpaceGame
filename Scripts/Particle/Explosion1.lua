
-- Explosion1


Explosion1 = {}; 
Explosion1.intensity = 30; 
Explosion1.rotation = math.random(0,360); 
Explosion1.force = 20; 
Explosion1.dampening = .85; 
Explosion1.growth = 1.2;

if(math.random( 1, 2) == 1) then 
    Explosion1.id = 3;
else
    Explosion1.id = 1; 
end