import React, { useState, useRef, useCallback } from 'react';
import { Link, useHistory } from 'react-router-dom';
import Form, {
    Item,
    Label,
    ButtonItem,
    ButtonOptions,
    RequiredRule,
    //CustomRule,
    //EmailRule
} from 'devextreme-react/form';
import notify from 'devextreme/ui/notify';
import LoadIndicator from 'devextreme-react/load-indicator';
import apiClient from '../../api/apiClient';
import './create-account-form.scss';

export default function CreateAccountForm(props) {
    const history = useHistory();
    const [loading, setLoading] = useState(false);
    const formData = useRef({});

    const onSubmit = useCallback(async (e) => {
        e.preventDefault();
        const { email } = formData.current;
        setLoading(true);

        const result = await apiClient.register(email);
        setLoading(false);

        if (result.isSuccessful) {
            history.push('/login');
        } else {
            notify(result.reason, 'error', 2000);
        }
    }, [history]);

    //const confirmPassword = useCallback(
    //    ({ value }) => value === formData.current.password,
    //    []
    //);

    return (
        <form className={'create-account-form'} onSubmit={onSubmit}>
            <Form formData={formData.current} disabled={loading}>
                <Item
                    dataField={'email'}
                    editorType={'dxTextBox'}
                    editorOptions={emailEditorOptions}
                >
                    <RequiredRule message="Benutzername ist erforderlich" />
                    <Label visible={false} />
                </Item>
                {/*<Item*/}
                {/*    dataField={'password'}*/}
                {/*    editorType={'dxTextBox'}*/}
                {/*    editorOptions={passwordEditorOptions}*/}
                {/*>*/}
                {/*    <RequiredRule message="Password is required" />*/}
                {/*    <Label visible={false} />*/}
                {/*</Item>*/}
                {/*<Item*/}
                {/*    dataField={'confirmedPassword'}*/}
                {/*    editorType={'dxTextBox'}*/}
                {/*    editorOptions={confirmedPasswordEditorOptions}*/}
                {/*>*/}
                {/*    <RequiredRule message="Password is required" />*/}
                {/*    <CustomRule*/}
                {/*        message={'Passwords do not match'}*/}
                {/*        validationCallback={confirmPassword}*/}
                {/*    />*/}
                {/*    <Label visible={false} />*/}
                {/*</Item>*/}
                {/*<Item>*/}
                {/*    <div className='policy-info'>*/}
                {/*        By creating an account, you agree to the <Link to="#">Terms of Service</Link> and <Link to="#">Privacy Policy</Link>*/}
                {/*    </div>*/}
                {/*</Item>*/}
                <ButtonItem>
                    <ButtonOptions
                        width={'100%'}
                        type={'default'}
                        useSubmitBehavior={true}
                    >
                        <span className="dx-button-text">
                            {
                                loading
                                    ? <LoadIndicator width={'24px'} height={'24px'} visible={true} />
                                    : 'Konto anlegen'
                            }
                        </span>
                    </ButtonOptions>
                </ButtonItem>
                <Item>
                    <div className={'login-link'}>
                        Haben Sie ein Konto? <Link to={'/login'}>Anmelden</Link>
                    </div>
                </Item>
            </Form>
        </form>
    );
}

const emailEditorOptions = { stylingMode: 'filled', placeholder: 'Benutzername'/*, mode: 'email'*/ };
//const passwordEditorOptions = { stylingMode: 'filled', placeholder: 'Password', mode: 'password' };
//const confirmedPasswordEditorOptions = { stylingMode: 'filled', placeholder: 'Confirm Password', mode: 'password' };
