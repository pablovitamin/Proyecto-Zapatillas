#include <DistanceSensor.hpp>

DistanceSensor *sensor1;
DistanceSensor *sensor2;

byte c0 = 0xFF;
byte c1 = 0xBE;
byte c2 = 0xFF;

byte info_sensor[2] = {0, 0};

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  sensor1 = CreateDistanceSensor(9,8);
  sensor2 = CreateDistanceSensor(7,6);
  
}

void PrintData()
{
  /*
  int s = sensor1->GetDistance();
  if(s < 130)
  {
    Serial.println("pillado");
  }
  */
  
  //Serial.print("cm: ");
  //Serial.print(sensor1->GetDistance());
  //Serial.print(" : ");
  Serial.println(sensor1->GetDistance());
  Serial.println(sensor2->GetDistance());
  
}

void SendData()
{
  Serial.flush();
  
  Serial.write(c0);
  Serial.write(c1);
  Serial.write(c2);

  info_sensor[0] = sensor1->GetDistance();
  info_sensor[1] = sensor2->GetDistance();

  
  Serial.write(info_sensor, sizeof(info_sensor));
}

void loop() {

  PrintData();
  //SendData();
  
  delay(15);
}
