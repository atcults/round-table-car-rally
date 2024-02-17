# Car Rally Calculation Explained

This application is designed for round table car rally calculations. It takes a speed chart and marshal points as inputs to prepare a calculated chart. In the first step, it generates a chart with the expected times to reach each marshal point based on the provided speed limits. The second step involves recording the actual arrival times at each marshal point and calculating the car's points based on any penalties incurred for arriving too early or late.

## SPEED REFERENCE CHART (SR)

| REFERENCE | FROM-KM | TO-KM | AVERAGE-SPEED |
|-----------|---------|-------|---------------|
| SR1       | 00      | 07    | 27            |
| SR2       | 07      | 17    | 30            |
| SR3       | 17      | 28    | 25            |
| SR4       | 28      | 38    | 31            |
| SR5       | 38      | 41.5  | 23            |
| SR6       | 41.5    | 47    | 20            |
| SR7       | 47      | 53.5  | 26            |
| SR8       | 53.5    | 58.4  | 18            |

## MARSHAL POINTS (MP)

| MARSHAL | KM   | BREAK-MINUTES |
|---------|------|---------------|
| MP1     | 00   | 00            |
| MP2     | 08   | 00            |
| MP3     | 17.3 | 00            |
| MP4     | 26.1 | 00            |
| MP5     | 38.4 | 15            |
| MP6     | 42.8 | 00            |
| MP7     | 47.4 | 00            |
| MP8     | 58.4 | 00            |

## Combined Speed Required Chart for Marshal Points

MP1 is the start point. We need to calculate the time required to reach MP2, which is 8KM away from MP1. According to the speed reference chart (SR), there are two speed references between MP1 and MP2. From 0 to 7 KM, the average speed is 27 (SR1), and from 7 to 17 KM, the speed is 30 (SR2). The total time to reach from MP1 to MP2 is calculated as follows:

((07-00) * 60) / 27 = 15.56 minutes for the first segment
((08-07) * 60) / 30 = 2 minutes for the second segment
Total = 15.56 + 2 = 17.56 minutes, which rounds to 18 minutes to reach MP2.


### Breakup Chart

| POINT  | CD     | DOF   | SPEED | TTR    | MT   | BT   |
|--------|--------|-------|-------|--------|------|------|
| Start  | 0      |       |       |        |      |      |
|        | 7      | 7     | 27    | 15.56  |      |      |
| MP1    | 8      | 1     | 30    | 2.00   | 18   | 0    |
|        | 17     | 9     | 30    | 18.00  |      |      |
| MP2    | 17.3   | 0.3   | 25    | 0.72   | 19   | 0    |
| MP3    | 26.1   | 8.8   | 25    | 21.12  | 21   | 0    |
|        | 28     | 1.9   | 25    | 4.56   |      |      |
|        | 38     | 10    | 31    | 19.35  |      |      |
| MP4    | 38.4   | 0.4   | 23    | 1.04   | 25   | 15   |
|        | 41.5   | 3.1   | 23    | 8.09   |      |      |
| MP5    | 42.8   | 1.3   | 20    | 3.90   | 12   | 0    |
|        | 47     | 4.2   | 20    | 12.60  |      |      |
| MP6    | 47.4   | 0.4   | 26    | 0.92   | 14   | 0    |
|        | 53.5   | 6.1   | 26    | 14.08  |      |      |
| Finish | 58.4   | 4.9   | 18    | 16.33  | 30   | 0    |
| TOTAL  | 58.4   |       |       | 138.27 | 138  |      |


## Calculating Proxy Time

When a driver misses any marshal point, the time is not captured. In this case, the system should calculate the probable time, which is the previously captured time (or proxy time), and add the time required to reach the missed point, marking the time as a proxy.

### Explanation for Proxy Time Calculation:

The proxy time is calculated for both MP1 and the START point.

### Example 1:

| POINTS | TIME    | PROXY-TIME | TIME-TAKEN | TIME-REQUIRED | LATE/EARLY | POINTS |
|--------|---------|------------|------------|---------------|------------|--------|
| MP1    | 8:06:00 | 8:06:00    |            |               |            |        |
| MP2    | 8:54:00 |            |            |               |            |        |
| MP3    | 9:19:00 |            |            |               |            |        |
| MP4    |         |            |            |               |            |        |
| MP5    | 10:37:00|            |            |               |            |        |
| MP6    |        
