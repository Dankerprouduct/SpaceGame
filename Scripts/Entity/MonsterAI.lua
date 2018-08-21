

radius = 32 * 20

Attack = false;
function Update() 

	
	 if(DistanceTo(GetPlayer()) <  radius) then 
		
		
		UpdatePhysics();  
		if(Attack == false) then -- follow player if youre not attacking
			AddSteeringForce( Pursue(GetPlayer())); 			
		end
		
			if(DistanceTo(GetPlayer()) < 128 * 2) then 
			-- Attack			
			Attack = true; 

			if(math.random(1,2) == 2) then
				FireAt(GetPlayer(), 1);
			else
				FireAt(GetPlayer(), 0);
			end

			AddSteeringForce(Separation())
			if(DistanceTo(GetPlayer()) < 128) then -- dont let player get too close
				AddSteeringForce(Evade(GetPlayer()));
			end

		else 
			Attack = false; 
			
		end

	 end

 end