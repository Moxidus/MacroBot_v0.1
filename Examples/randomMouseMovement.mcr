DELAY(5000)
FOR i = 0 TO 20 STEP 0.05 THEN
	DELAY(1)
	VAR x = 500*(0.3*(-3.2*SIN(-1.3*i)-1.2*SIN(-1.7*2.718*i)+1.9*SIN(0.7*PI*i))+2)
	VAR y = 250*(0.3*(-3.2*SIN(-1.6*i-153)-1.2*SIN(-1.5*2.718*i-153)+1.9*SIN(0.3*PI*i-153))+2)
	MOVE(x, y)
	PRINT(i)
END







