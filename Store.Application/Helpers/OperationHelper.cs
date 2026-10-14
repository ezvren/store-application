using Store.Application.DTOs;
using Store.Core.Enum;

namespace Store.Application.Helpers
{
    public class OperationHelper
    {
        private readonly List<decimal> bills = [500, 200, 100, 50, 20];
        private readonly List<decimal> coins = [10, 5, 1, 0.50m, 0.20m, 0.10m];

        public async Task<List<DenominationBreakdownDTO>> CreateCashOperation(decimal receivedAmount)
        {
            List<decimal> denominations = [.. bills, .. coins];
            List<DenominationBreakdownDTO> breakdown = [];

            foreach (var denomination in denominations)
            {
                if (receivedAmount <= 0)
                    break;

                DenominationBreakdownDTO? result = CalculateDenomination(receivedAmount, denomination);

                if (result is null) continue;

                breakdown.Add(result);

                receivedAmount -= result.Quantity * result.DenominationValue;

                if (receivedAmount == 0)
                    break;
            }

            return breakdown;
        }

        public DenominationBreakdownDTO? CalculateDenomination(decimal totalAmount, decimal denomination)
        {
            if (totalAmount < denomination)
                return null;

            int quantity = (int)decimal.Floor(totalAmount / denomination);

            return new DenominationBreakdownDTO
            {
                DenominationValue = denomination,
                Quantity = quantity,
                DenominationKind = bills.Contains(denomination)  ? DenominationKind.Bill : DenominationKind.Coin
            };
        }
    }
}
