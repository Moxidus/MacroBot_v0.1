{"savedImages":[],"script":"DELAY(5000)\r\nFOR i = 0 TO 20 STEP 0.05 THEN\r\n\tDELAY(1)\r\n\tVAR x = 500*(0.3*(-3.2*SIN(-1.3*i)-1.2*SIN(-1.7*2.718*i)\u002B1.9*SIN(0.7*PI*i))\u002B2)\r\n\tVAR y = 250*(0.3*(-3.2*SIN(-1.6*i-153)-1.2*SIN(-1.5*2.718*i-153)\u002B1.9*SIN(0.3*PI*i-153))\u002B2)\r\n\tMOVE(x, y)\r\n\tPRINT(i)\r\nEND\r\n\r\n\r\n\r\n"}