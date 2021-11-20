import * as api from '@actual-app/api';
import { send } from '@actual-app/api/connection';

module.exports = {
    send,
    ...api
}
