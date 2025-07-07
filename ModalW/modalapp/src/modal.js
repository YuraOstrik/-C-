import 'bootstrap/dist/css/bootstrap.min.css';
import { useState } from 'react';
import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';
import Modal from 'react-bootstrap/Modal';

export function Example(){
  const [show, setShow] = useState(false);

  const handleClose = () => setShow(false);
  const handleShow = () => setShow(true);

  return (
    <>
      <Button variant="primary" onClick={handleShow}>
        Launch demo modal
      </Button>

      <Modal show={show} onHide={handleClose} centered>
        <Modal.Header closeButton>
          <Modal.Title className="w-100 text-center">Авторизація</Modal.Title>
        </Modal.Header>
        
        <Modal.Body>
        <Form>
            <Form.Group className="mb-4">
            <Form.Control
                type="text"
                placeholder="Ім'я"
                autoFocus
                className="form-control-lg rounded-pill px-4"
            />
            </Form.Group>
            <Form.Group className="mb-4" controlId="exampleForm.ControlInput1">
            <Form.Control
                type="email"
                placeholder="Електронна пошта"
                className="form-control-lg rounded-pill px-4"
            />
            </Form.Group>
            <div className="text-center">
                <a href="#" className="forgot-password">Забули пароль?</a>
            </div>
        </Form>
        </Modal.Body>

       <Modal.Footer className="d-flex flex-column gap-3">
        <Button
            type="button"
            className="btn btn-primary btn-lg rounded-pill px-4"
            onClick={handleClose}
            style={{ width: '100%' }}
        >
            Увійти
        </Button>

        <Button
            type="button"
            className="btn btn-light btn-lg rounded-pill px-4"
            onClick={handleClose}
            style={{ width: '100%' }}
        >
            Авторизуватися через Google
        </Button>
        </Modal.Footer>



      </Modal>
    </>
  );
}


