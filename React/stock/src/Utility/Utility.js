class Utility
{
    currencyFormatter = new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD',
        minimumFractionDigits: 2,
        maximumFractionDigits: 2,
    });

    FormatCurrency(num) {
        return this.currencyFormatter.format(num);
    }
}

const utility = new Utility();
export default utility;