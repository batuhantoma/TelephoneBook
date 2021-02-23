import React, { useState, useEffect } from "react";
import { Grid, TextField, withStyles, FormControl, InputLabel, Select, MenuItem, Button, FormHelperText } from "@material-ui/core";
import useForm from "./useForm";
import { connect } from "react-redux";
import * as actions from "../actions/dCandidate";
import { useToasts } from "react-toast-notifications";
import axios from "axios";

const styles = theme => ({
    root: {
        '& .MuiTextField-root': {
            margin: theme.spacing(1),
            minWidth: 230,
        }
    },
    formControl: {
        margin: theme.spacing(1),
        minWidth: 230,
    },
    smMargin: {
        margin: theme.spacing(1)
    }
})

const initialFieldValues = {
    Ad: '',
    Soyad: '',
    Firma: '',
    TelefonNumarasi: '',
    EmailAdresi: '',
    Konum: '',
    RaporcuAd: ''
}

const DCandidateForm = ({ classes, ...props }) => {

    //toast msg.
    const { addToast } = useToasts()

    //validate()
    //validate({fullName:'jenny'})
    const validate = (fieldValues = values) => {
        let temp = { ...errors }
        if ('Ad' in fieldValues)
            temp.Ad = fieldValues.Ad ? "" : "This field is required."
        if ('Soyad' in fieldValues)
            temp.Soyad = fieldValues.Soyad ? "" : "This field is required."
        if ('Firma' in fieldValues)
            temp.Firma = fieldValues.Firma ? "" : "This field is required."
        if ('TelefonNumarasi' in fieldValues)
            temp.TelefonNumarasi = fieldValues.TelefonNumarasi ? "" : "This field is required."
        if ('EmailAdresi' in fieldValues) {
            temp.EmailAdresi = (/^$|.+@.+..+/).test(fieldValues.EmailAdresi) ? "" : "Email is not valid."
        }

        if ('Konum' in fieldValues)
            temp.Konum = fieldValues.Konum ? "" : "This field is required."

        if ('RaporcuAd' in fieldValues) {
            temp.RaporcuAd = fieldValues.RaporcuAd ? "" : "This field is required."
        }

        setErrors({
            ...temp
        })

        if (fieldValues == values) return Object.values(temp).every(x => x == "")
    }

    const {
        values,
        setValues,
        errors,
        setErrors,
        handleInputChange,
        resetForm
    } = useForm(initialFieldValues, validate, props.setCurrentId)

    //material-ui select
    // const inputLabel = React.useRef(null);
    // const [labelWidth, setLabelWidth] = React.useState(0);
    // React.useEffect(() => {
    // setLabelWidth(inputLabel.current.offsetWidth);
    //}, []);

    const handleSubmit = e => {
        e.preventDefault()
        if (validate()) {
            const onSuccess = () => {
                resetForm()
                addToast("Submitted successfully", { appearance: 'success' })
            }
            if (props.currentId == 0)
                props.createDCandidate(values, onSuccess)
            else
                props.updateDCandidate(props.currentId, values, onSuccess)
        }
    }

    const sendReport = e => {
        let report ={Ad:values.RaporcuAd}
        axios.post('https://localhost:44329/ReportRequest', report)
            .then()
            .catch(error => {
                console.error('There was an error!', error);
            });
    }

    const refreshGrid = e => {
        debugger;
        props.fetchCandidateReports();
        // fetchAllDCandidatesReports.reportList

        // let report ={Ad:values.RaporcuAd}
        // axios.post('https://localhost:44329/ReportRequest', report)
        //     .then()
        //     .catch(error => {
        //         console.error('There was an error!', error);
        //     });
    }

    

    useEffect(() => {
        if (props.currentId != 0) {
            setValues({
                ...props.dCandidateList.find(x => x.id == props.currentId)
            })
            setErrors({})
        }
    }, [props.currentId])

    return (
        <form autoComplete="off" noValidate className={classes.root} onSubmit={handleSubmit}>
            <Grid container direction="row">
                <Grid item xs={6}>
                    <TextField
                        name="Ad"
                        variant="outlined"
                        label="Ad"
                        value={values.Ad}
                        onChange={handleInputChange}
                        {...(errors.Ad && { error: true, helperText: errors.Ad })}
                    />
                    <TextField
                        name="Soyad"
                        variant="outlined"
                        label="Soyad"
                        value={values.Soyad}
                        onChange={handleInputChange}
                        {...(errors.Soyad && { error: true, helperText: errors.Soyad })}
                    />
                    <TextField
                        name="Firma"
                        variant="outlined"
                        label="Firma"
                        value={values.Firma}
                        onChange={handleInputChange}
                        {...(errors.Firma && { error: true, helperText: errors.Firma })}
                    />
                    <TextField
                        name="TelefonNumarasi"
                        variant="outlined"
                        label="TelefonNumarasi"
                        value={values.TelefonNumarasi}
                        onChange={handleInputChange}
                        {...(errors.TelefonNumarasi && { error: true, helperText: errors.TelefonNumarasi })}
                    />


                </Grid>
                <Grid item xs={6}>
                    <TextField
                        name="EmailAdresi"
                        variant="outlined"
                        label="EmailAdresi"
                        value={values.EmailAdresi}
                        onChange={handleInputChange}
                        {...(errors.EmailAdresi && { error: true, helperText: errors.EmailAdresi })}
                    />
                    <TextField
                        name="Konum"
                        variant="outlined"
                        label="Konum"
                        value={values.Konum}
                        onChange={handleInputChange}
                        {...(errors.Konum && { error: true, helperText: errors.Konum })}
                    />
                    <div>
                        <Button
                            variant="contained"
                            color="primary"
                            type="submit"
                            className={classes.smMargin}
                        >
                            Submit
                        </Button>
                        <Button
                            variant="contained"
                            className={classes.smMargin}
                            onClick={resetForm}
                        >
                            Reset
                        </Button>
                    </div>
                </Grid>
            </Grid>

            <ColoredLine color="black" />

            <Grid container direction="row">
                <Grid item xs={6}>
                    <TextField
                        name="RaporcuAd"
                        variant="outlined"
                        label="RaporcuAd"
                        value={values.RaporcuAd}
                        onChange={handleInputChange}
                    />
                </Grid>
                <Grid item xs={6}>
                    <div>
                        <Button
                            variant="contained"
                            color="primary"

                            className={classes.smMargin}
                            id="Report"
                            onClick={sendReport}
                        >
                            Submit
                        </Button>
                        <Button
                            variant="contained"
                            className={classes.smMargin}
                            id="Refresh"
                            onClick={refreshGrid}
                        >
                            Refresh
                        </Button>
                    </div>
                </Grid>

            </Grid>
        </form>
    );
}

const ColoredLine = ({ color }) => (
    <hr
        style={{
            color: color,
            backgroundColor: color,
            height: 5
        }}
    />
);

const mapStateToProps = state => ({
    dCandidateList: state.dCandidate.list,
    reportList : state.dCandidate.listReport
})

const mapActionToProps = {
    fetchCandidateReports: actions.fetchAll_Report,
    createDCandidate: actions.create,
    updateDCandidate: actions.update
}

export default connect(mapStateToProps, mapActionToProps)(withStyles(styles)(DCandidateForm));