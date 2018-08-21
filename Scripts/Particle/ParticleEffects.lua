ParticleEffects = {};

-- Laser 1
ParticleEffects.Laser1 = {};
ParticleEffects.Laser1.Position = GetCenter(); 
ParticleEffects.Laser1.Rotation = GetMouseDirection();
ParticleEffects.Laser1.Force = math.random(30, 60);
ParticleEffects.Laser1.Dampening =  .95;
ParticleEffects.Laser1.Intensity = 30; 
ParticleEffects.Laser1.TextureID = 0; 

-- Laser 2
ParticleEffects.Laser2 = {};
ParticleEffects.Laser2.Position = GetCenter(); 
ParticleEffects.Laser2.Rotation = GetMouseDirection();
ParticleEffects.Laser2.Force = 20;
ParticleEffects.Laser2.Dampening = .85; 
ParticleEffects.Laser1.Intensity = 3; 
ParticleEffects.Laser2.TextureID = 0; 


--Thruster
ParticleEffects.Thruster1 = {};
ParticleEffects.Thruster1.Position = GetCenter(); 
ParticleEffects.Thruster1.Rotation = GetMouseDirection(); 
ParticleEffects.Thruster1.Force = math.random(10, 30) + (GetForce() * 1.5);
ParticleEffects.Thruster1.Dampening = .85; 
ParticleEffects.Thruster1.Intensity = 3; 
ParticleEffects.Thruster1.TextureID = math.random(0, 5);
if(math.random(0, 1) == 1) then 
	ParticleEffects.Thruster1.TextureID = 0;
else
	ParticleEffects.Thruster1.TextureID = 1 ;
end
 
 -- Blue Laser
ParticleEffects.BlueLaser = {};
ParticleEffects.BlueLaser.Position = GetCenter(); 
ParticleEffects.BlueLaser.Rotation = GetMouseDirection();
ParticleEffects.BlueLaser.Force = math.random(64, 128);
ParticleEffects.BlueLaser.Dampening = .99; 
ParticleEffects.BlueLaser.Intensity = 1; 
ParticleEffects.BlueLaser.TextureID = 6

-- Explostion 1
ParticleEffects.Explostion1 = {};
ParticleEffects.Explostion1.Position = GetCenter(); 
ParticleEffects.Explostion1.Rotation = math.random() + math.random(1, 99);
ParticleEffects.Explostion1.Force = math.random(64, 128);
ParticleEffects.Explostion1.Dampening = .85; 
ParticleEffects.Explostion1.Intensity = 10; 
ParticleEffects.Explostion1.TextureID = 6

