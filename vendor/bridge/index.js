const api = require('@actual-app/api');

// export default {
//     init: api.init,
//     disconnect: api.disconnect,
//     loadBudget: api.loadBudget,
//     getBudgetMonths: api.getBudgetMonths
// }

(async () => {
    await api.init();
    await api.loadBudget('My-Finances-1-dde9556');
    let result = await api.getBudgetMonths();
    console.log(result);
    // await api.disconnect();
})();
