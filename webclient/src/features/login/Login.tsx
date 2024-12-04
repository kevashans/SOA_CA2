import { Card, Input, PasswordInput } from "@mantine/core";
import { IconAt } from '@tabler/icons-react';

export const Login = () => {
    return <Card title="Login">
          <Input placeholder="Your email" leftSection={<IconAt size={16} />} />
          <PasswordInput
      label="Input label"
      description="Input description"
      placeholder="Input placeholder"
    />
    </Card>
}