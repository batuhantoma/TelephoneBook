import React, { useState, useEffect } from "react";
import { connect } from "react-redux";
import * as actions from "../actions/dCandidate";
import { Grid, Paper, TableContainer, Table, TableHead, TableRow, TableCell, TableBody, withStyles, ButtonGroup, Button, TextField } from "@material-ui/core";
import DCandidateForm from "./DCandidateForm";
import EditIcon from "@material-ui/icons/Edit";
import DeleteIcon from "@material-ui/icons/Delete";
import { useToasts } from "react-toast-notifications";




const styles = theme => ({
    root: {
        "& .MuiTableCell-head": {
            fontSize: "1.25rem"
        }
    },
    innerRoot: {
        '& .MuiTextField-root': {
            margin: theme.spacing(1),
            minWidth: 230,
        },
        borderWidth: 2,
    },
    paper: {
        margin: theme.spacing(2),
        padding: theme.spacing(2)
    },
    button: {
        marginTop: 15
    }
})
const initialFieldValues = {
    Ad: ''

}

function sendReport(e) {

    console.log(e)
    // let report = {UUID }
    // axios.post('https://localhost:44329/ReportRequest', report)
    // .then()
    // .catch(error => {
    //     console.error('There was an error!', error);
    // });
}

const DCandidates = ({ classes, ...props }) => {
    const [currentId, setCurrentId] = useState(0)

    useEffect(() => {
        props.fetchAllDCandidates()
    }, [])//componentDidMount

    useEffect(() => {
        props.fetchAllDCandidatesReports()
    }, [])//componentDidMount

    //toast msg.
    const { addToast } = useToasts()

    const onDelete = id => {
        if (window.confirm('Are you sure to delete this record?'))
            props.deleteDCandidate(id, () => addToast("Deleted successfully", { appearance: 'info' }))
    }
    return (
        <Paper className={classes.paper} elevation={6}>
            <Grid container direction="column">
                <Grid container >
                    <Grid item xs={6}>
                        <DCandidateForm {...({ currentId, setCurrentId })} />
                    </Grid>
                    <Grid item xs={6}>
                        <TableContainer>
                            <Table>
                                <TableHead className={classes.root}>
                                    <TableRow>
                                        <TableCell>Ad</TableCell>
                                        <TableCell>Soyad</TableCell>
                                        <TableCell>Firma</TableCell>
                                        <TableCell>Telefon</TableCell>
                                        <TableCell>EmailAdresi</TableCell>
                                        <TableCell>Konum</TableCell>
                                    </TableRow>
                                </TableHead>
                                <TableBody>
                                    {
                                        props.dCandidateList.map((record, index) => {
                                            return (<TableRow key={index} hover>
                                                <TableCell>{record.ad}</TableCell>
                                                <TableCell>{record.soyad}</TableCell>
                                                <TableCell>{record.firma}</TableCell>
                                                <TableCell>{record.iletisimBilgisi.bilgiTipi.telefonNumarasi}</TableCell>
                                                <TableCell>{record.iletisimBilgisi.bilgiTipi.emailAdresi}</TableCell>
                                                <TableCell>{record.iletisimBilgisi.bilgiTipi.konum}</TableCell>
                                                <TableCell>
                                                    <ButtonGroup variant="text">
                                                        <Button><EditIcon color="primary"
                                                            onClick={() => {
                                                                setCurrentId(record.id)
                                                            }} /></Button>
                                                        <Button><DeleteIcon color="secondary"
                                                            onClick={() => onDelete(record.id)} /></Button>
                                                    </ButtonGroup>
                                                </TableCell>
                                            </TableRow>)
                                        })
                                    }
                                </TableBody>
                            </Table>
                        </TableContainer>
                    </Grid>
                </Grid>


                <Grid container className={classes.innerRoot}>
                    <TableContainer>
                        <Table>
                            <TableHead className={classes.root}>
                                <TableRow>
                                <TableCell>UID</TableCell>
                                    <TableCell>Ad</TableCell>
                                    <TableCell>Rapor Tarihi</TableCell>
                                    <TableCell>Rapor Durumu</TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {
                                    props.reportList.map((record, index) => {
                                        return (<TableRow key={index} hover>
                                            <TableCell>{record.uuid}</TableCell>
                                            <TableCell>{record.ad}</TableCell>
                                            <TableCell>{record.raporTarihi}</TableCell>
                                            <TableCell>{record.raporDurumu}</TableCell>
                                        </TableRow>)
                                    })
                                }
                            </TableBody>
                        </Table>
                    </TableContainer>

                </Grid>
            </Grid>
        </Paper>
    );
}

const mapStateToProps = state => ({
    dCandidateList: state.dCandidate.list,
    reportList:state.dCandidate.listReport
})

const mapActionToProps = {
    fetchAllDCandidates: actions.fetchAll,
    fetchAllDCandidatesReports: actions.fetchAll_Report,
    deleteDCandidate: actions.Delete
}

export default connect(mapStateToProps, mapActionToProps)(withStyles(styles)(DCandidates));