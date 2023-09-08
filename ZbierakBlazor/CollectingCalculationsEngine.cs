namespace ZbierakWeb
{
    public class CollectingCalculationsEngine
    {
        private readonly int _collectingLevel;
        private readonly int _armyCapacity;

        const double durationExponent = 0.45;
        const double durationFactor = 0.77220749;
        const double durationInitialSeconds = 1800;

        const double s1Factor = 0.1;
        const double s2Factor = 0.25;
        const double s3Factor = 0.5;
        const double s4Factor = 0.75;

        private Dictionary<int, double> StagesValues;

        public CollectingCalculationsEngine(int CollectingLevel, int ArmyCapacity)
        {
            _collectingLevel = CollectingLevel;
            _armyCapacity = ArmyCapacity;
            StagesValues = new Dictionary<int, double>() 
            {
                {1, 0},
                {2, 0},
                {3, 0},
                {4, 0}
            };
        }

        public Dictionary<int,double> CalculateOptimalArmyDistributionWithEqualTime()
        {
            if (_collectingLevel == 1)
            {
                StagesValues[1] = _armyCapacity;
            }
            if (_collectingLevel == 2)
            {
                var secondOnlyTime = CalculateExpeditionTime(_armyCapacity, s2Factor);
                var secondAndFirstTime = CalculateExpeditionTime((double)_armyCapacity * 2.5 / 3.5, s1Factor);
                
                var secondOnlyResources = s2Factor * _armyCapacity;
                var secondAndFirstResources = (s2Factor * (double)_armyCapacity * 2.5 / 3.5) + (s1Factor * (double)_armyCapacity * 1.5 / 3.5);
                
                var secondOnlyResourcesPerHour = secondOnlyResources / secondOnlyTime * 3600;
                var secondAndFirstResourcesPerHour = secondAndFirstResources / secondAndFirstTime * 3600;
                
                if (secondOnlyResourcesPerHour > secondAndFirstResourcesPerHour)
                {
                    StagesValues[2] = _armyCapacity;
                }
                else
                {
                    StagesValues[1] = _armyCapacity * 2.5 / 3.5;
                    StagesValues[2] = _armyCapacity / 3.5;
                }
            }
            if (_collectingLevel == 3)
            {
                var thirdOnlyTime = CalculateExpeditionTime(_armyCapacity, s3Factor);
                var thirdAndSecondTime = CalculateExpeditionTime((double)_armyCapacity * 2.5 / 7.5, s3Factor);
                var thirdAndSecondAndFirstTime = CalculateExpeditionTime((double)_armyCapacity * 5 / 8, s1Factor);

                var thirdOnlyResources = s3Factor * _armyCapacity;
                var thirdAndSecondResources = (s3Factor * (double)_armyCapacity * 2.5 / 7.5) + (s2Factor * (double)_armyCapacity * 5 / 7.5);
                var thirdAndSecondAndFirstResources = (s3Factor * (double)_armyCapacity / 8) + (s2Factor * (double)_armyCapacity / 4) + (s1Factor * (double)_armyCapacity * 5 / 8);
               
                var thirdOnlyResourcesPerHour = thirdOnlyResources / thirdOnlyTime * 3600;
                var thirdAndSecondResourcesPerHour = thirdAndSecondResources / thirdAndSecondTime * 3600;
                var thirdAndSecondAndFirstResourcesPerHour = thirdAndSecondAndFirstResources / thirdAndSecondAndFirstTime * 3600;
                
                if (thirdOnlyResourcesPerHour > thirdAndSecondResourcesPerHour && thirdOnlyResourcesPerHour > thirdAndSecondAndFirstResourcesPerHour)
                {
                    StagesValues[3] = _armyCapacity;
                }
                else if (thirdAndSecondResourcesPerHour > thirdAndSecondAndFirstResourcesPerHour)
                {
                    StagesValues[3] = _armyCapacity * 2.5 / 7.5;
                    StagesValues[2] = _armyCapacity * 5 / 7.5;
                }
                else
                {
                    StagesValues[3] = _armyCapacity / 8;
                    StagesValues[2] = _armyCapacity / 4;
                    StagesValues[1] = _armyCapacity * 5 / 8;
                }
            }
            if (_collectingLevel == 4)
            {
                var fourthOnlyTime = CalculateExpeditionTime(_armyCapacity, s4Factor);
                var fourthAndThirdTime = CalculateExpeditionTime((double)_armyCapacity * 5 / 12.5, s4Factor);
                var fourthAndThirdAndSecondTime = CalculateExpeditionTime((double)_armyCapacity / 5.5, s4Factor);
                var fourthAndThirdAndSecondAndFirstTime = CalculateExpeditionTime((double)_armyCapacity / 13, s4Factor);

                var fourthOnlyResources = s4Factor * _armyCapacity;
                var fourthAndThirdResources = (s4Factor * (double)_armyCapacity * 2.5 / 7.5) + (s3Factor * (double)_armyCapacity * 5 / 7.5);
                var fourthAndThirdAndSecondResources = (s4Factor * (double)_armyCapacity / 5.5) + (s3Factor * (double)_armyCapacity * 1.5 / 5.5)
                    + (s2Factor * (double)_armyCapacity * 3 / 5.5);
                var fourthAndThirdAndSecondAndFirstResources = (s4Factor * (double)_armyCapacity / 13) + (s3Factor * (double)_armyCapacity * 1.5 / 13)
                    + (s2Factor * (double)_armyCapacity * 3 / 13) + (s1Factor * (double)_armyCapacity * 7.5 / 13);

                var fourthOnlyResourcesPerHour = fourthOnlyResources / fourthOnlyTime * 3600;
                var fourthAndThirdResourcesPerHour = fourthAndThirdResources / fourthAndThirdTime * 3600;
                var fourthAndThirdAndSecondResourcesPerHour = fourthAndThirdAndSecondResources / fourthAndThirdAndSecondTime * 3600;
                var fourthAndThirdAndSecondAndFirstResourcesPerHour = fourthAndThirdAndSecondAndFirstResources / fourthAndThirdAndSecondAndFirstTime * 3600;
                
                if (fourthOnlyResourcesPerHour > fourthAndThirdResourcesPerHour && fourthOnlyResourcesPerHour > fourthAndThirdAndSecondResourcesPerHour
                    && fourthOnlyResourcesPerHour > fourthAndThirdAndSecondAndFirstResourcesPerHour)
                {
                    StagesValues[4] = _armyCapacity;
                }
                else if (fourthAndThirdResourcesPerHour > fourthAndThirdAndSecondResourcesPerHour
                    && fourthAndThirdResourcesPerHour > fourthAndThirdAndSecondAndFirstResourcesPerHour)
                {
                    StagesValues[4] = _armyCapacity * 2.5 / 7.5;
                    StagesValues[3] = _armyCapacity * 5 / 7.5;
                }
                else if (fourthAndThirdAndSecondResourcesPerHour > fourthAndThirdAndSecondAndFirstResourcesPerHour)
                {
                    StagesValues[4] = _armyCapacity / 5.5;
                    StagesValues[3] = _armyCapacity * 1.5 / 5.5;
                    StagesValues[2] = _armyCapacity * 3 / 5.5;
                }
                else
                {
                    StagesValues[4] = _armyCapacity / 13;
                    StagesValues[3] = _armyCapacity * 1.5 / 13;
                    StagesValues[2] = _armyCapacity * 3 / 13;
                    StagesValues[1] = _armyCapacity * 7.5 / 13;
                }
            }
            return StagesValues;
        }

        private double CalculateExpeditionTime(double capacity, double capacityFactor)
        {
            return (Math.Pow(Math.Pow(capacity, 2) * 100 * Math.Pow(capacityFactor, 2), durationExponent) + durationInitialSeconds) * durationFactor;
        }
    }
}
