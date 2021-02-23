import axios from "axios";

const baseUrl = "https://localhost:44329/"
const baseUrl_Report = "https://localhost:44329/"



export default {

    dCandidate(url = baseUrl + 'kisiler/') {
        return {
            fetchAll: () => axios.get(url),
            fetchAll_Report: () => axios.get(baseUrl_Report+"ReportRequest"),
            fetchById: id => axios.get(url + id),
            create: newRecord => axios.post(url, newRecord),
            update: (id, updateRecord) => axios.put(url + id, updateRecord),
            delete: id => axios.delete(url + id)
        }
    }
}